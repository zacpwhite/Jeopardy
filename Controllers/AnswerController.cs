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

        public async Task<IActionResult> Index(int id)
        {
            try
            {
                var answer = await _context.Answers
                    .Include(x => x.Questions)
                    .FirstOrDefaultAsync(x => x.AnswerId == id);

                if (answer == null)
                {
                    return NotFound();
                }

                var answerVM = _mapper.Map<AnswerViewModel>(answer);
                return PartialView("_AnswerPartial", answerVM);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An exception occurred in {MethodBase.GetCurrentMethod().Name}");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Complete(int id)
        {
            try
            {
                var answer = await _context.Answers.FirstOrDefaultAsync(x => x.AnswerId == id);

                if (answer == null) {
                    return NotFound();
                }

                answer.HasBeenRead = true;
                _context.Update(answer);
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