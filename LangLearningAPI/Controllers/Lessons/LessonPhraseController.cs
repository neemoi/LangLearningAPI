using Application.DtoModels.Lessons.Phrasees;
using Application.Services.Interfaces.IServices.Lesons;
using Common.Exceptions;
using LangLearningAPI.Exceptions;
using Microsoft.AspNetCore.Mvc;

namespace LangLearningAPI.Controllers.Lessons
{
    [Route("api/[controller]")]
    [ApiController]
    public class LessonPhraseController : ControllerBase
    {
        private readonly ILessonPhraseService _phraseService;
        private readonly ILogger<LessonPhraseController> _logger;

        public LessonPhraseController(ILessonPhraseService phraseService, ILogger<LessonPhraseController> logger)
        {
            _phraseService = phraseService;
            _logger = logger;
        }

        [HttpGet]
        [ProducesResponseType(typeof(List<LessonPhraseDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<List<LessonPhraseDto>>> GetAllPhrases()
        {
            try
            {
                return Ok(await _phraseService.GetAllPhrasesAsync());
            }
            catch (KeyNotFoundException ex)
            {
                _logger.LogWarning(ex, "No phrases found");
                return NotFound();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting phrases");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpGet("{phraseId:int}")]
        [ProducesResponseType(typeof(LessonPhraseDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<LessonPhraseDto>> GetPhraseById(int phraseId)
        {
            try
            {
                return Ok(await _phraseService.GetPhraseByIdAsync(phraseId));
            }
            catch (KeyNotFoundException ex)
            {
                _logger.LogWarning(ex, "Phrase {PhraseId} not found", phraseId);
                return NotFound();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting phrase {PhraseId}", phraseId);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpPost]
        [ProducesResponseType(typeof(LessonPhraseDto), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<LessonPhraseDto>> CreatePhrase([FromBody] CreateLessonPhraseDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var createdPhrase = await _phraseService.CreatePhraseAsync(dto);
                return CreatedAtAction(nameof(GetPhraseById), new { phraseId = createdPhrase.Id }, createdPhrase);
            }
            catch (ServiceException ex) when (ex.Message.Contains("Lesson not found"))
            {
                _logger.LogWarning(ex, "Lesson {LessonId} not found", dto.LessonId);
                return NotFound(new
                {
                    title = "Not Found",
                    status = StatusCodes.Status404NotFound,
                    detail = "Lesson not found",
                    lessonId = dto.LessonId
                });
            }
            catch (ServiceException ex) when (ex.Message.Contains("Invalid phrase data"))
            {
                _logger.LogWarning(ex, "Invalid data when creating phrase");
                return BadRequest(new
                {
                    title = "Bad Request",
                    status = StatusCodes.Status400BadRequest,
                    detail = "Invalid phrase data",
                    errors = ModelState.Values.SelectMany(v => v.Errors)
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating phrase in lesson {LessonId}", dto.LessonId);
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    title = "An error occurred while processing your request.",
                    status = StatusCodes.Status500InternalServerError,
                    detail = "An unexpected error occurred"
                });
            }
        }

        [HttpPut]
        [ProducesResponseType(typeof(LessonPhraseDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<LessonPhraseDto>> UpdatePhrase([FromBody] UpdateLessonPhraseDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                return Ok(await _phraseService.UpdatePhraseAsync(dto));
            }
            catch (ServiceException ex) when (ex.Message.Contains("Phrase not found"))
            {
                _logger.LogWarning(ex, "Phrase {PhraseId} not found", dto.PhraseId);
                return NotFound(new
                {
                    title = "Not Found",
                    status = StatusCodes.Status404NotFound,
                    detail = "Phrase not found",
                    phraseId = dto.PhraseId
                });
            }
            catch (ServiceException ex) when (ex.Message.Contains("Invalid phrase data"))
            {
                _logger.LogWarning(ex, "Invalid data when updating phrase");
                return BadRequest(new
                {
                    title = "Bad Request",
                    status = StatusCodes.Status400BadRequest,
                    detail = "Invalid phrase data",
                    errors = ModelState.Values.SelectMany(v => v.Errors)
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating phrase {PhraseId}", dto.PhraseId);
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    title = "An error occurred while processing your request.",
                    status = StatusCodes.Status500InternalServerError,
                    detail = "An unexpected error occurred"
                });
            }
        }

        [HttpDelete("{phraseId:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeletePhrase(int phraseId)
        {
            try
            {
                return Ok(await _phraseService.DeletePhraseAsync(phraseId));
            }
            catch (KeyNotFoundException ex)
            {
                _logger.LogWarning(ex, "Phrase {PhraseId} not found", phraseId);
                return NotFound();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting phrase {PhraseId}", phraseId);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
    }
}
