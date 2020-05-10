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
        public async Task<IActionResult> Index(int gameId, int round)
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
                        CurrentRound = (Round)round,
                        Categories = g.Categories
                            .Where(c => c.Round == (Round)round)
                            .Select(c => new Category
                            {
                                CategoryId = c.CategoryId,
                                CategoryDescription = c.CategoryDescription,
                                CategorySortOrder = c.CategorySortOrder,
                                CategoryTitle = c.CategoryTitle,
                                Answers = c.Answers.OrderBy(a => a.AnswerValue).ToList()
                            }).OrderBy(c => c.CategorySortOrder).ToList()
                    })
                    .FirstOrDefaultAsync(x => x.GameId == gameId);

                if (game == null)
                {
                    _logger.LogWarning($"Game with id {gameId} for round {round} not found");
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
        public async Task<IActionResult> Create(NewGameViewModel newGameViewModel)
        {
            try
            {
                var game = _mapper.Map<Game>(newGameViewModel);
                var user = _mapper.Map<User>(newGameViewModel);

                _context.Games.Add(game);
                await _context.SaveChangesAsync();

                user.GameId = game.GameId;
                _context.Users.Add(user);
                await _context.SaveChangesAsync();

                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An exception occurred in {MethodBase.GetCurrentMethod().Name}");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
    }
}