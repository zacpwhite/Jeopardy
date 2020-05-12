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
        public async Task<IActionResult> Index(int id)
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

                    var user = new User() {
                        Username = joinGameVM.Username
                    };

                    _context.Users.Add(user);
                    await _context.SaveChangesAsync();
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