using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using AutoMapper;
using Jeopardy.Data;
using Jeopardy.Models;
using Jeopardy.Models.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Jeopardy.Utilities;
using Newtonsoft.Json;

namespace Jeopardy.Controllers
{
    public class GameController : Controller
    {
        private ILogger<GameController> _logger;

        private JeopardyContext _context;

        private IMapper _mapper;

        public GameController(JeopardyContext context, IMapper mapper, ILogger<GameController> logger)
        {
            _context = context;
            _mapper = mapper;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> Start(int id)
        {
            try
            {
                var userCookieString = Request.Cookies["user"];
                if (string.IsNullOrEmpty(userCookieString))
                {
                    Response.Redirect(Url.Action("Index", "Home"));
                }

                var userCookie = userCookieString.FromBase64JsonString<User>();
                if (userCookie == null)
                {
                    Response.Redirect(Url.Action("Index", "Home"));
                }

                var user = await _context.Users.FirstOrDefaultAsync(x => x.UserId.Equals(userCookie.UserId) && x.GameId.Equals(id));
                if (user == null)
                {
                    Response.Redirect(Url.Action("Index", "Home"));
                }

                var game = await _context.Games.FirstOrDefaultAsync(x => x.GameId.Equals(id));

                if (game == null)
                {
                    Response.Redirect(Url.Action("Index", "Home"));
                }

                var gameVM = _mapper.Map<GameViewModel>(game);

                return View("Start", gameVM);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An exception occurred in {MethodBase.GetCurrentMethod().Name}");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpGet]
        public async Task<IActionResult> Play(int id)
        {
            try
            {
                var game = await _context.Games
                    .Include(x => x.Categories)
                    .ThenInclude(x => x.Answers)
                    .ThenInclude(x => x.Questions)
                    .AsNoTracking()
                    .Select(g => new Game
                    {
                        GameId = g.GameId,
                        GameDescription = g.GameDescription,
                        GameTitle = g.GameTitle,
                        CurrentRound = Round.Jeopardy,
                        Categories = g.Categories
                            .Where(c => c.Round == Round.Jeopardy)
                            .Select(c => new Category
                            {
                                CategoryId = c.CategoryId,
                                CategoryDescription = c.CategoryDescription,
                                CategorySortOrder = c.CategorySortOrder,
                                CategoryTitle = c.CategoryTitle,
                                Answers = c.Answers.OrderBy(a => a.AnswerValue).ToList()
                            }).OrderBy(c => c.CategorySortOrder).ToList()
                    })
                    .FirstOrDefaultAsync(x => x.GameId == id);

                if (game == null)
                {
                    _logger.LogWarning($"Game with id {id} for round {Round.Jeopardy} not found");
                    return NotFound();
                }

                var gameVM = _mapper.Map<GameViewModel>(game);

                return View(gameVM);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An exception occurred in {MethodBase.GetCurrentMethod().Name}");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpGet("{gameId}.{format?}"), FormatFilter]
        public async Task<IActionResult> Template(int gameId)
        {
            try
            {
                var game = await _context.Games
                    .Include(x => x.Categories)
                    .ThenInclude(x => x.Answers)
                    .AsNoTracking()
                    .Select(g => new Game
                    {
                        GameId = g.GameId,
                        GameDescription = g.GameDescription,
                        GameTitle = g.GameTitle,
                        Categories = g.Categories
                            .Select(c => new Category
                            {
                                CategoryId = c.CategoryId,
                                CategoryDescription = c.CategoryDescription,
                                CategorySortOrder = c.CategorySortOrder,
                                CategoryTitle = c.CategoryTitle,
                                Answers = c.Answers
                                    .Select(a => new Answer
                                    {
                                        AnswerId = a.AnswerId,
                                        AnswerText = a.AnswerText,
                                        AnswerValue = a.AnswerValue,
                                        Questions = a.Questions.OrderBy(q => q.QuestionId).ToList()
                                    }).OrderBy(a => a.AnswerValue).ToList()
                            }).OrderBy(c => c.CategorySortOrder).ToList()
                    })
                    .FirstOrDefaultAsync(x => x.GameId == gameId);

                if (game == null)
                {
                    _logger.LogWarning($"Game with id {gameId} not found");
                    return NotFound();
                }

                return Ok(game);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An exception occurred in {MethodBase.GetCurrentMethod().Name}");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpGet]
        public IActionResult Create()
        {
            var newGameVM = new NewGameViewModel();
            return PartialView("_NewGamePartial", newGameVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(NewGameViewModel newGameVM)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var game = _mapper.Map<Game>(newGameVM);
                    var user = _mapper.Map<User>(newGameVM);

                    _context.Games.Add(game);
                    await _context.SaveChangesAsync();

                    user.GameId = game.GameId;
                    user.UserIsHost = true;
                    _context.Users.Add(user);
                    await _context.SaveChangesAsync();

                    var cookieOptions = new CookieOptions
                    {
                        Expires = DateTime.UtcNow.AddDays(1)
                    };

                    Response.Cookies.Append("user", user.ToJsonBase64String(), cookieOptions);
                    return Ok(Url.Action("Start", "Game", new {id=game.GameId}));
                }
                return PartialView("_NewGamePartial", newGameVM);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An exception occurred in {MethodBase.GetCurrentMethod().Name}");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpGet("search")]
        [Produces("application/json")]
        public async Task<IActionResult> Search()
        {
            try
            {
                string term = HttpContext.Request.Query["term"].ToString();

                var games = await _context.Games
                    .Where(x => x.GameTitle.Contains(term))
                    .Select(x => new
                    {
                        gameId = x.GameId,
                        gameTitle = x.GameTitle
                    }).ToListAsync();

                return Ok(games);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An exception occurred in {MethodBase.GetCurrentMethod().Name}");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpGet]
        public IActionResult Join()
        {
            var joinGameVM = new JoinGameViewModel();
            return PartialView("_JoinGamePartial", joinGameVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Join(JoinGameViewModel joinGameVM)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var game = await _context.Games.FirstOrDefaultAsync(x => x.GameId.Equals(joinGameVM.GameId));

                    if (game == null)
                    {
                        return NotFound();
                    }

                    var user = new User()
                    {
                        Username = joinGameVM.Username,
                        GameId = game.GameId
                    };

                    _context.Users.Add(user);
                    await _context.SaveChangesAsync();

                    var cookieOptions = new CookieOptions
                    {
                        Expires = DateTime.UtcNow.AddHours(1)
                    };

                    Response.Cookies.Append("user", user.ToJsonBase64String(), cookieOptions);
                    return Ok(Url.Action("Start", "Game", new {id=game.GameId}));
                }

                return PartialView("_JoinGamePartial", joinGameVM);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An exception occurred in {MethodBase.GetCurrentMethod().Name}");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
    }
}