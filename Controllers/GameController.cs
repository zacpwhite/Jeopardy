using System;
using System.Collections.Generic;
using System.Linq;
using Jeopardy.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Jeopardy.Controllers
{
  public class GameController : Controller
  {
    ILogger<GameController> _logger;

    public GameController(ILogger<GameController> logger)
    {
      _logger = logger;
    }

    public IActionResult Index()
    {
        var categories = new List<CategoryViewModel>();

        for(var i = 1; i <= 6; i++) {
            categories.Add(new CategoryViewModel(){
                CategoryId = i,
                CategoryName = $"Category {i}",
                CategoryOrder = i
            });
        }

        foreach(var category in categories) {
            var answers = new List<AnswerViewModel>();
            for(var i = 1; i <= 5; i++) {
                answers.Add(new AnswerViewModel() {
                    AnswerId = i,
                    AnswerValue = i * 100,
                    Answer = $"This is answer {i} in {category.CategoryName}"
                });
            }
            category.Answers = answers;
        }

        var game = new GameViewModel() {
            GameId = 1,
            GameName = "Test Game",
            Categories = categories
        };

        return View(game);
    }
  }
}