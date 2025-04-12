using Application.DtoModels.Lessons.QuizQuestion;
using Application.Services.Interfaces.IServices.Lesons;
using LangLearningAPI.Exceptions;
using Microsoft.AspNetCore.Mvc;

namespace LangLearningAPI.Controllers.Lessons
{
    [Route("api/QuizQuestion")]
    [ApiController]
    public class LessonQuizQuestionController : ControllerBase
    {
        private readonly IQuizQuestionService _quizQuestionService;
        private readonly ILogger<LessonQuizQuestionController> _logger;

        public LessonQuizQuestionController(IQuizQuestionService quizQuestionService, ILogger<LessonQuizQuestionController> logger)
        {
            _quizQuestionService = quizQuestionService;
            _logger = logger;
        }

        [HttpPost]
        public async Task<IActionResult> CreateQuizQuestion([FromBody] CreateQuizQuestionDto dto)
        {
            try
            {
                var createdQuestion = await _quizQuestionService.CreateQuizQuestionAsync(dto);
                return CreatedAtAction(nameof(GetQuizQuestionById), new { id = createdQuestion.Id }, createdQuestion);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating quiz question");
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetQuizQuestionById(int id)
        {
            try
            {
                return Ok(await _quizQuestionService.GetQuizQuestionByIdAsync(id));
            }
            catch (NotFoundException ex)
            {
                _logger.LogWarning(ex, "Quiz question not found");
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching quiz question");
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetAllQuizQuestions()
        {
            try
            {
                return Ok(await _quizQuestionService.GetAllQuizQuestionsAsync());
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching all quiz questions");
                return BadRequest(ex.Message);
            }
        }

        [HttpPut]
        public async Task<IActionResult> UpdateQuizQuestion([FromBody] UpdateQuizQuestionDto dto)
        {
            if (dto.QuizId != dto.QuizId)
            {
                _logger.LogWarning($"Quiz ID in route ({dto.QuizId}) does not match the ID in the body ({dto.QuizId})");
                return BadRequest("Quiz ID mismatch");
            }

            try
            {
                return Ok(await _quizQuestionService.UpdateQuizQuestionAsync(dto));
            }
            catch (NotFoundException)
            {
                _logger.LogWarning($"Quiz question with ID {dto.QuizId} not found for update");
                return NotFound();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating quiz question");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteQuizQuestion(int id)
        {
            try
            {
                await _quizQuestionService.DeleteQuizQuestionAsync(id);
                return NoContent();
            }
            catch (NotFoundException ex)
            {
                _logger.LogWarning(ex, "Quiz question not found for deletion");
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting quiz question");
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("quiz/{quizId}")]
        public async Task<IActionResult> GetQuizQuestionsByQuizId(int quizId)
        {
            try
            {
                return Ok(await _quizQuestionService.GetQuizQuestionsByQuizIdAsync(quizId));
            }
            catch (NotFoundException ex)
            {
                _logger.LogWarning(ex, "Quiz or questions not found");
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching quiz questions");
                return BadRequest(ex.Message);
            }
        }
    }
}
