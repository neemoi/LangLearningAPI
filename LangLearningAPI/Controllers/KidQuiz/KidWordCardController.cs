using Application.DtoModels.KidQuiz.KidWordCard;
using Application.Services.Interfaces.IServices.KidQuiz;
using Microsoft.AspNetCore.Mvc;

namespace LangLearningAPI.Controllers.KidQuiz
{
    [Route("api/[controller]")]
    [ApiController]
    public class KidWordCardController : ControllerBase
    {
        private readonly IKidWordCardService _kidWordCardService;
        private readonly ILogger<KidWordCardController> _logger;

        public KidWordCardController(IKidWordCardService kidWordCardService, ILogger<KidWordCardController> logger)
        {
            _kidWordCardService = kidWordCardService;
            _logger = logger;
        }

        [HttpPost]
        public async Task<IActionResult> CreateKidWordCard(CreateKidWordCardDto dto)
        {
            try
            {
                return Ok(await _kidWordCardService.CreateWordCardAsync(dto));
            }
            catch (KeyNotFoundException ex)
            {
                _logger.LogWarning(ex, "KidLesson not found while creating KidWordCard.");
                return NotFound(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while creating KidWordCard.");
                return StatusCode(500, new { message = "An unexpected error occurred." });
            }
        }

        [HttpGet("{id}", Name = "GetWordCardById")]
        public async Task<IActionResult> GetWordCardByIdAsync(int id)
        {
            try
            {
                var wordCard = await _kidWordCardService.GetWordCardByIdAsync(id);
                if (wordCard == null)
                {
                    return NotFound($"KidWordCard with ID {id} not found.");
                }

                return Ok(wordCard);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching KidWordCard.");
                return StatusCode(500, "Internal server error.");
            }
        }

        [HttpGet("lesson/{lessonId}")]
        public async Task<IActionResult> GetWordCardsByLessonIdAsync(int lessonId)
        {
            try
            {
                var wordCards = await _kidWordCardService.GetWordCardsByLessonIdAsync(lessonId);
                if (wordCards == null || !wordCards.Any())
                {
                    return NotFound($"No KidWordCards found for Lesson ID {lessonId}.");
                }

                return Ok(wordCards);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching KidWordCards for lesson.");
                return StatusCode(500, "Internal server error.");
            }
        }

        [HttpPut]
        public async Task<IActionResult> UpdateWordCardAsync([FromBody] UpdateKidWordCardDto dto)
        {
            if (dto == null)
            {
                return BadRequest("Invalid data.");
            }

            try
            {
                var updatedWordCard = await _kidWordCardService.UpdateWordCardAsync(dto.Id, dto);
                if (updatedWordCard == null)
                {
                    return NotFound($"KidWordCard with ID {dto.Id} not found.");
                }

                return Ok(updatedWordCard);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while updating KidWordCard.");
                return StatusCode(500, "Internal server error.");
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteWordCardAsync(int id)
        {
            try
            {
                var result = await _kidWordCardService.DeleteWordCardAsync(id);
                if (!result)
                {
                    return NotFound($"KidWordCard with ID {id} not found.");
                }

                return Ok(new { message = $"KidWordCard with ID {id} deleted successfully." });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while deleting KidWordCard.");
                return StatusCode(500, "Internal server error.");
            }
        }
    }
}
