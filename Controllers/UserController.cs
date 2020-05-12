using System;
using System.Linq;
using Jeopardy.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Jeopardy.Controllers
{
  public class UserController : Controller
  {
    ILogger<UserController> _logger;

    JeopardyContext _context;

    public UserController(ILogger<UserController> logger, JeopardyContext context)
    {
      _logger = logger;
      _context = context;
    }

    public IActionResult Index()
    {      
      return View();
    }

    [AcceptVerbs("GET", "POST")]
    [Produces("application/json")]
    public IActionResult ValidateUsername(string username, int gameId)
    {
        var exists = _context.Users.Any(x => x.GameId.Equals(gameId) && x.Username.ToLower().Trim().Contains(username.ToLower().Trim()));
        return Json(!exists);
    }
  }
}