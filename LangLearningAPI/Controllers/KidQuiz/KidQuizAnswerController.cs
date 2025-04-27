using Application.DtoModels.KidQuiz.KidQuizAnswer;
using Application.Services.Interfaces.IServices.KidQuiz;
using Microsoft.AspNetCore.Mvc;

namespace LangLearningAPI.Controllers.KidQuiz
{
    [ApiController]
    [Route("api/[controller]")]
    public class KidQuizAnswerController : ControllerBase
    {
        private readonly IKidQuizAnswerService _kidQuizAnswerService;
        private readonly ILogger<KidQuizAnswerController> _logger;

        public KidQuizAnswerController(IKidQuizAnswerService kidQuizAnswerService, ILogger<KidQuizAnswerController> logger)
        {
            _kidQuizAnswerService = kidQuizAnswerService;
            _logger = logger;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetKidQuizAnswerById(int id)
        {
            var answer = await _kidQuizAnswerService.GetKidQuizAnswerByIdAsync(id);

            if (answer == null)
            {
                _logger.LogWarning($"KidQuizAnswer with id {id} not found.");
                return NotFound();
            }

            return Ok(answer);
        }

        [HttpGet("question/{questionId}")]
        public async Task<IActionResult> GetAnswersByQuestionId(int questionId)
        {
            var answers = await _kidQuizAnswerService.GetAnswersByQuestionIdAsync(questionId);

            if (answers == null || !answers.Any())
            {
                _logger.LogWarning($"No answers found for QuestionId {questionId}.");
                return NotFound();
            }

            return Ok(answers);
        }

        [HttpPost]
        public async Task<IActionResult> CreateAnswer([FromBody] CreateKidQuizAnswerDto dto)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Invalid model state while creating KidQuizAnswer.");
                return BadRequest(ModelState);
            }

            var createdAnswer = await _kidQuizAnswerService.AddKidQuizAnswerAsync(dto);

            if (createdAnswer == null)
            {
                _logger.LogWarning($"Cannot create KidQuizAnswer: Question with ID {dto.QuestionId} does not exist.");
                return BadRequest($"Cannot create answer because question with ID {dto.QuestionId} does not exist.");
            }

            return CreatedAtAction(
                nameof(GetKidQuizAnswerById),
                new { id = createdAnswer.Id },
                createdAnswer
            );
        }

        [HttpPut]
        public async Task<IActionResult> UpdateKidQuizAnswer([FromBody] UpdateKidQuizAnswerDto dto)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Invalid model state while updating KidQuizAnswer.");
                return BadRequest(ModelState);
            }

            var updatedAnswer = await _kidQuizAnswerService.UpdateKidQuizAnswerAsync(dto);

            if (updatedAnswer == null)
            {
                _logger.LogWarning($"Failed to update KidQuizAnswer with id {dto.Id}.");
                return NotFound();
            }

            return Ok(updatedAnswer);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAnswer(int id)
        {
            var deletedAnswer = await _kidQuizAnswerService.DeleteKidQuizAnswerAsync(id);

            if (deletedAnswer == null)
            {
                _logger.LogWarning($"Failed to delete KidQuizAnswer with id {id}.");
                return NotFound();
            }

            return Ok(deletedAnswer);
        }
    }
}