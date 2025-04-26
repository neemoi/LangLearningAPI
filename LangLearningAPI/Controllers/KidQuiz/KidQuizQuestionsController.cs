using Application.DtoModels.KidQuiz;
using Application.DtoModels.KidQuiz.KidQuizQuestion;
using Application.Services.Interfaces.IServices.KidQuiz;
using Microsoft.AspNetCore.Mvc;

namespace LangLearningAPI.Controllers.KidQuiz
{
    [Route("api/[controller]")]
    [ApiController]
    public class KidQuizQuestionsController : ControllerBase
    {
        private readonly IKidQuizQuestionService _kidQuizQuestionService;
        private readonly ILogger<KidQuizQuestionsController> _logger;

        public KidQuizQuestionsController(IKidQuizQuestionService kidQuizQuestionService, ILogger<KidQuizQuestionsController> logger)
        {
            _kidQuizQuestionService = kidQuizQuestionService;
            _logger = logger;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetByIdAsync(int id)
        {
            try
            {
                var question = await _kidQuizQuestionService.GetByIdQuizQuestionAsync(id);
                if (question == null)
                {
                    return NotFound(new { Message = $"KidQuizQuestion with ID {id} not found." });
                }

                return Ok(question);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in GetByIdAsync for KidQuizQuestion with ID {id}: {ex.Message}");
                return StatusCode(500, new { Message = "An error occurred while retrieving the question." });
            }
        }

        [HttpGet("byLesson/{lessonId}")]
        public async Task<IActionResult> GetByLessonIdAsync(int lessonId)
        {
            try
            {
                var questions = await _kidQuizQuestionService.GetByLessonIdQuizQuestionAsync(lessonId);
                if (!questions.Any())
                {
                    return NotFound(new { Message = $"No questions found for Lesson ID {lessonId}." });
                }

                return Ok(questions);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in GetByLessonIdAsync for Lesson ID {lessonId}: {ex.Message}");
                return StatusCode(500, new { Message = "An error occurred while retrieving the questions." });
            }
        }

        [HttpPost]
        public async Task<IActionResult> AddAsync([FromBody] CreateKidQuizQuestionDto questionDto)
        {
            try
            {
                var addedQuestion = await _kidQuizQuestionService.AddQuizQuestionAsync(questionDto);
                if (addedQuestion == null)
                {
                    return BadRequest(new { Message = "Failed to add KidQuizQuestion." });
                }

                var locationUrl = $"/api/KidQuizQuestions/{addedQuestion.Id}";

                return Created(locationUrl, addedQuestion);
            }
            catch (ArgumentException ex)
            {
                _logger.LogWarning(ex, "Validation error during AddAsync");
                return BadRequest(new { Message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in AddAsync for KidQuizQuestion: {ex.Message}");
                return StatusCode(500, new { Message = "An error occurred while adding the question." });
            }
        }

        [HttpPut]
        public async Task<IActionResult> UpdateAsync([FromBody] UpdateKidQuizQuestionDto questionDto)
        {
            try
            {
                if (questionDto.Id != questionDto.Id)
                    return BadRequest("ID in route does not match ID in body");

                var updatedQuestion = await _kidQuizQuestionService.UpdateQuizQuestionAsync(questionDto.Id, questionDto);
                return updatedQuestion == null
                    ? NotFound(new { Message = $"Question with ID {questionDto.Id} not found." })
                    : Ok(updatedQuestion);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating question with ID {QuestionId}", questionDto.Id);
                return StatusCode(500, new { Message = "An error occurred while updating the question." });
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            try
            {
                var deletedQuestion = await _kidQuizQuestionService.DeleteQuizQuestionAsync(id);
                if (deletedQuestion == null)
                {
                    return NotFound(new { Message = $"KidQuizQuestion with ID {id} not found for deletion." });
                }

                return Ok(deletedQuestion);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in DeleteAsync for KidQuizQuestion with ID {id}: {ex.Message}");
                return StatusCode(500, new { Message = "An error occurred while deleting the question." });
            }
        }
    }
}
