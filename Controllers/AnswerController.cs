using System;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using AutoMapper;
using Jeopardy.Data;
using Jeopardy.Models.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Jeopardy.Controllers
{
    [Route("[controller]")]
    public class AnswerController : Controller
    {

        ILogger<AnswerController> _logger;

        private JeopardyContext _context;

        private IMapper _mapper;

        public AnswerController(JeopardyContext context, IMapper mapper, ILogger<AnswerController> logger)
        {
            _logger = logger;
            _context = context;
            _mapper = mapper;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Index(int id)
        {
            try
            {
                var answer = await _context.Answers.FirstOrDefaultAsync(x => x.AnswerId == id);
                var answerVM = _mapper.Map<AnswerViewModel>(answer);
                return PartialView("_AnswerPartial", answerVM);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An exception occurred in {MethodBase.GetCurrentMethod().Name}");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
    }
}