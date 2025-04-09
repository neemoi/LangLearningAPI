using Application.DtoModels.Lessons.Words;
using Application.Services.Interfaces.IServices.Lesons;
using Microsoft.AspNetCore.Mvc;
using LangLearningAPI.Exceptions;

namespace LangLearningAPI.Controllers.Lessons
{
    [Route("api/words")]
    [ApiController]
    public class LessonWordController : ControllerBase
    {
        private readonly ILessonWordService _lessonWordService;
        private readonly ILogger<LessonWordController> _logger;

        public LessonWordController(ILessonWordService lessonWordService, ILogger<LessonWordController> logger)
        {
            _lessonWordService = lessonWordService;
            _logger = logger;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetAllWords()
        {
            try
            {
                return Ok(await _lessonWordService.GetAllWordsAsync());
            }
            catch (ArgumentException ex)
            {
                _logger.LogWarning(ex, "Invalid request for all words");
                return BadRequest(CreateProblemDetails(
                    StatusCodes.Status400BadRequest,
                    "Invalid Request",
                    ex.Message));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error getting all words");
                return StatusCode(StatusCodes.Status500InternalServerError,
                    CreateProblemDetails(
                        StatusCodes.Status500InternalServerError,
                        "Server Error",
                        "An unexpected error occurred while processing your request"));
            }
        }

        [HttpGet("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetWordById(int id)
        {
            try
            {
                return Ok(await _lessonWordService.GetWordByIdAsync(id));
            }
            catch (KeyNotFoundException ex)
            {
                _logger.LogWarning(ex, "Word not found: {Id}", id);
                return NotFound(CreateProblemDetails(
                    StatusCodes.Status404NotFound,
                    "Not Found",
                    ex.Message));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error getting word: {Id}", id);
                return StatusCode(StatusCodes.Status500InternalServerError,
                    CreateProblemDetails(
                        StatusCodes.Status500InternalServerError,
                        "Server Error",
                        "An unexpected error occurred while processing your request"));
            }
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status409Conflict)]
        public async Task<IActionResult> CreateWord([FromBody] CreateLessonWordDto dto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    _logger.LogWarning("Invalid model state for word creation");
                    return BadRequest(ModelState);
                }

                var createdWord = await _lessonWordService.CreateWordAsync(dto);
                return CreatedAtAction(nameof(GetWordById), new { id = createdWord.Id }, createdWord);
            }
            catch (ValidationException ex)
            {
                _logger.LogWarning(ex, "Validation error creating word");
                return BadRequest(CreateProblemDetails(
                    StatusCodes.Status400BadRequest,
                    "Validation Error",
                    ex.Message));
            }
            catch (ConflictException ex)
            {
                _logger.LogWarning(ex, "Conflict creating word");
                return Conflict(CreateProblemDetails(
                    StatusCodes.Status409Conflict,
                    "Conflict",
                    ex.Message));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error creating word");
                return StatusCode(StatusCodes.Status500InternalServerError,
                    CreateProblemDetails(
                        StatusCodes.Status500InternalServerError,
                        "Server Error",
                        "An unexpected error occurred while processing your request"));
            }
        }

        [HttpPatch("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateWord(int id, [FromBody] UpdateLessonWordDto dto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    _logger.LogWarning("Invalid model state for word update");
                    return BadRequest(ModelState);
                }

                return Ok(await _lessonWordService.UpdatePartialWordsAsync(id, dto));
            }
            catch (KeyNotFoundException ex)
            {
                _logger.LogWarning(ex, "Word not found for update: {Id}", id);
                return NotFound(CreateProblemDetails(
                    StatusCodes.Status404NotFound,
                    "Not Found",
                    ex.Message));
            }
            catch (ValidationException ex)
            {
                _logger.LogWarning(ex, "Validation error updating word: {Id}", id);
                return BadRequest(CreateProblemDetails(
                    StatusCodes.Status400BadRequest,
                    "Validation Error",
                    ex.Message));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error updating word: {Id}", id);
                return StatusCode(StatusCodes.Status500InternalServerError,
                    CreateProblemDetails(
                        StatusCodes.Status500InternalServerError,
                        "Server Error",
                        "An unexpected error occurred while processing your request"));
            }
        }

        [HttpDelete("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteWord(int id)
        {
            try
            {
                return Ok(await _lessonWordService.DeleteWordsAsync(id));
            }
            catch (KeyNotFoundException ex)
            {
                _logger.LogWarning(ex, "Word not found for deletion: {Id}", id);
                return NotFound(CreateProblemDetails(
                    StatusCodes.Status404NotFound,
                    "Not Found",
                    ex.Message));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error deleting word: {Id}", id);
                return StatusCode(StatusCodes.Status500InternalServerError,
                    CreateProblemDetails(
                        StatusCodes.Status500InternalServerError,
                        "Server Error",
                        "An unexpected error occurred while processing your request"));
            }
        }

        private static ProblemDetails CreateProblemDetails(int status, string title, string detail)
        {
            return new ProblemDetails
            {
                Status = status,
                Title = title,
                Detail = detail
            };
        }
    }
}