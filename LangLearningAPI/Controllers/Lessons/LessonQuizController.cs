using Application.DtoModels.Lessons.Quiz;
using Application.Services.Interfaces.IServices.Lesons;
using Common.Exceptions;
using LangLearningAPI.Exceptions;
using Microsoft.AspNetCore.Mvc;

namespace LangLearningAPI.Controllers
{
    [Route("api/LessonQuiz")]
    [ApiController]
    public class LessonQuizController : ControllerBase
    {
        private readonly IQuizService _quizService;
        private readonly ILogger<LessonQuizController> _logger;

        public LessonQuizController(IQuizService quizService, ILogger<LessonQuizController> logger)
        {
            _quizService = quizService;
            _logger = logger;
        }

        [HttpPost]
        public async Task<ActionResult<QuizDto>> CreateQuizAsync([FromBody] CreateQuizDto createQuizDto)
        {
            try
            {
                var createdQuiz = await _quizService.CreateQuizAsync(createQuizDto);
                return CreatedAtAction(
                    actionName: nameof(GetQuizById),
                    routeValues: new { id = createdQuiz.Id },
                    value: createdQuiz);
            }
            catch (ServiceException ex) when (ex.ErrorCode == "VALIDATION_ERROR")
            {
                _logger.LogError(ex, "Validation error while creating quiz");
                return BadRequest(new { message = ex.Message, code = ex.ErrorCode });
            }
            catch (ServiceException ex) when (ex.ErrorCode == "DUPLICATE_QUIZ")
            {
                _logger.LogWarning(ex, "Duplicate quiz detected");
                return Conflict(new { message = ex.Message, code = ex.ErrorCode });
            }
            catch (NotFoundException ex)
            {
                _logger.LogWarning(ex, "Lesson not found");
                return NotFound(new { message = ex.Message, code = ex.ErrorCode });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error while creating quiz");
                return StatusCode(500, new { message = "An error occurred while processing your request." });
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<QuizDto>> GetQuizById(int id)
        {
            try
            {
                return Ok(await _quizService.GetQuizByIdAsync(id));
            }
            catch (NotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error getting quiz with ID {id}");
                return StatusCode(500, new { message = "An error occurred while processing your request." });
            }
        }

        [HttpPut]
        public async Task<ActionResult<QuizDto>> UpdateQuizAsync(UpdateQuizDto updateQuizDto)
        {
            try
            {
                return Ok(await _quizService.UpdateQuizAsync(updateQuizDto));
            }
            catch (NotFoundException ex)
            {
                _logger.LogWarning(ex, "Quiz or Lesson not found.");
                return NotFound(new { message = ex.Message });
            }
            catch (ServiceException ex)
            {
                _logger.LogError(ex, "Service error occurred while updating quiz.");
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected error occurred.");
                return StatusCode(500, new { message = "An error occurred while processing your request." });
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<QuizDto>> DeleteQuizAsync(int id)
        {
            try
            {
                var deletedQuiz = await _quizService.DeleteQuizAsync(id);
                return deletedQuiz == null ? NotFound() : Ok(deletedQuiz);
            }
            catch (NotFoundException ex)
            {
                _logger.LogWarning(ex, "Quiz not found.");
                return NotFound(new { message = ex.Message });
            }
            catch (ServiceException ex)
            {
                _logger.LogError(ex, "Error occurred while deleting quiz.");
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected error occurred.");
                return StatusCode(500, new { message = "An error occurred while processing your request." });
            }
        }

        [HttpGet("lesson/{lessonId}")]
        public async Task<ActionResult<IEnumerable<QuizDto>>> GetQuizzesByLessonIdAsync(int lessonId)
        {
            try
            {
                return Ok(await _quizService.GetQuizzesByLessonIdAsync(lessonId));
            }
            catch (ServiceException ex)
            {
                _logger.LogError(ex, "Error occurred while retrieving quizzes for lesson.");
                return StatusCode(500, new { message = "An error occurred while processing your request." });
            }
        }
    }
}
