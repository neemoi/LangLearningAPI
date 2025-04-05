using Application.DtoModels.Lessons.Lessons;
using Application.DtoModels.Lessons.Quiz;
using Application.Services.Interfaces.IServices;
using Microsoft.AspNetCore.Mvc;

namespace LangLearningAPI.Controllers.Lessons
{
    [Route("api/[controller]")]
    [ApiController]
    [Produces("application/json")]
    public class LessonsController : ControllerBase
    {
        private readonly ILessonService _lessonService;
        private readonly ILogger<LessonsController> _logger;

        public LessonsController(ILessonService lessonService, ILogger<LessonsController> logger)
        {
            _lessonService = lessonService;
            _logger = logger;
        }

        [HttpGet("{id:int}")]
        [ProducesResponseType(typeof(LessonDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<LessonDto>> GetLesson(int id)
        {
            try
            {
                return Ok(await _lessonService.GetLessonByIdAsync(id));
            }
            catch (ArgumentException ex)
            {
                _logger.LogWarning(ex, "Invalid lesson ID requested: {Id}", id);
                return BadRequest(CreateProblemDetails(
                    StatusCodes.Status400BadRequest,
                    "Invalid Request",
                    ex.Message));
            }
            catch (KeyNotFoundException ex)
            {
                _logger.LogWarning(ex, "Lesson not found: {Id}", id);
                return NotFound(CreateProblemDetails(
                    StatusCodes.Status404NotFound,
                    "Not Found",
                    ex.Message));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error getting lesson {Id}", id);
                return StatusCode(StatusCodes.Status500InternalServerError,
                    CreateProblemDetails(
                        StatusCodes.Status500InternalServerError,
                        "Server Error",
                        "An unexpected error occurred while processing your request"));
            }
        }

        [HttpGet]
        [ProducesResponseType(typeof(List<LessonDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<List<LessonDto>>> GetAllLessons()
        {
            try
            {
                return Ok(await _lessonService.GetAllLessonsAsync());
            }
            catch (KeyNotFoundException ex)
            {
                _logger.LogWarning(ex, "No lessons found");
                return NotFound(CreateProblemDetails(
                    StatusCodes.Status404NotFound,
                    "Not Found",
                    ex.Message));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error getting all lessons");
                return StatusCode(StatusCodes.Status500InternalServerError,
                    CreateProblemDetails(
                        StatusCodes.Status500InternalServerError,
                        "Server Error",
                        "An unexpected error occurred while processing your request"));
            }
        }

        [HttpPost]
        [ProducesResponseType(typeof(LessonDto), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<LessonDto>> CreateLesson([FromBody] CreateLessonDto createDto)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Invalid model state for lesson creation");
                return BadRequest(CreateProblemDetails(
                    StatusCodes.Status400BadRequest,
                    "Invalid Request",
                    ModelState));
            }

            try
            {
                var createdLesson = await _lessonService.CreateLessonAsync(createDto);
                return CreatedAtAction(nameof(GetLesson), new { id = createdLesson.Id }, createdLesson);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error creating lesson");
                return StatusCode(StatusCodes.Status500InternalServerError,
                    CreateProblemDetails(
                        StatusCodes.Status500InternalServerError,
                        "Server Error",
                        "An unexpected error occurred while processing your request"));
            }
        }

        [HttpPut("")]
        [ProducesResponseType(typeof(LessonDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<LessonDto>> UpdateLesson([FromBody] UpdateLessonDto updateDto)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Invalid model state for lesson update");
                return BadRequest(CreateProblemDetails(
                    StatusCodes.Status400BadRequest,
                    "Invalid Request",
                    ModelState));
            }

            if (updateDto.Id != updateDto.Id)
            {
                _logger.LogWarning("ID mismatch in lesson update. Route: {RouteId}, Body: {BodyId}", updateDto.Id, updateDto.Id);
                return BadRequest(CreateProblemDetails(
                    StatusCodes.Status400BadRequest,
                    "Invalid Request",
                    "The ID in the route does not match the ID in the request body"));
            }

            try
            {
                return Ok(await _lessonService.UpdateLessonAsync(updateDto));
            }
            catch (ArgumentException ex)
            {
                _logger.LogWarning(ex, "Invalid request for lesson update");
                return BadRequest(CreateProblemDetails(
                    StatusCodes.Status400BadRequest,
                    "Invalid Request",
                    ex.Message));
            }
            catch (KeyNotFoundException ex)
            {
                _logger.LogWarning(ex, "Lesson not found for update: {Id}", updateDto.Id);
                return NotFound(CreateProblemDetails(
                    StatusCodes.Status404NotFound,
                    "Not Found",
                    ex.Message));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error updating lesson {Id}", updateDto.Id);
                return StatusCode(StatusCodes.Status500InternalServerError,
                    CreateProblemDetails(
                        StatusCodes.Status500InternalServerError,
                        "Server Error",
                        "An unexpected error occurred while processing your request"));
            }
        }

        [HttpDelete("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeleteLesson(int id)
        {
            try
            {
                return Ok(await _lessonService.DeleteLessonAsync(id));
            }
            catch (ArgumentException ex)
            {
                _logger.LogWarning(ex, "Invalid request for lesson deletion: {Id}", id);
                return BadRequest(CreateProblemDetails(
                    StatusCodes.Status400BadRequest,
                    "Invalid Request",
                    ex.Message));
            }
            catch (KeyNotFoundException ex)
            {
                _logger.LogWarning(ex, "Lesson not found for deletion: {Id}", id);
                return NotFound(CreateProblemDetails(
                    StatusCodes.Status404NotFound,
                    "Not Found",
                    ex.Message));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error deleting lesson {Id}", id);
                return StatusCode(StatusCodes.Status500InternalServerError,
                    CreateProblemDetails(
                        StatusCodes.Status500InternalServerError,
                        "Server Error",
                        "An unexpected error occurred while processing your request"));
            }
        }

        [HttpGet("{lessonId:int}/words")]
        [ProducesResponseType(typeof(List<LessonWordDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<List<LessonWordDto>>> GetLessonWords(int lessonId)
        {
            try
            {
                return Ok(await _lessonService.GetLessonWordsAsync(lessonId));
            }
            catch (ArgumentException ex)
            {
                _logger.LogWarning(ex, "Invalid request for lesson words: {LessonId}", lessonId);
                return BadRequest(CreateProblemDetails(
                    StatusCodes.Status400BadRequest,
                    "Invalid Request",
                    ex.Message));
            }
            catch (KeyNotFoundException ex)
            {
                _logger.LogWarning(ex, "No words found for lesson: {LessonId}", lessonId);
                return NotFound(CreateProblemDetails(
                    StatusCodes.Status404NotFound,
                    "Not Found",
                    ex.Message));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error getting words for lesson {LessonId}", lessonId);
                return StatusCode(StatusCodes.Status500InternalServerError,
                    CreateProblemDetails(
                        StatusCodes.Status500InternalServerError,
                        "Server Error",
                        "An unexpected error occurred while processing your request"));
            }
        }

        [HttpGet("{lessonId:int}/phrases")]
        [ProducesResponseType(typeof(List<LessonPhraseDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<List<LessonPhraseDto>>> GetLessonPhrases(int lessonId)
        {
            try
            {
                return Ok(await _lessonService.GetLessonPhrasesAsync(lessonId));
            }
            catch (ArgumentException ex)
            {
                _logger.LogWarning(ex, "Invalid request for lesson phrases: {LessonId}", lessonId);
                return BadRequest(CreateProblemDetails(
                    StatusCodes.Status400BadRequest,
                    "Invalid Request",
                    ex.Message));
            }
            catch (KeyNotFoundException ex)
            {
                _logger.LogWarning(ex, "No phrases found for lesson: {LessonId}", lessonId);
                return NotFound(CreateProblemDetails(
                    StatusCodes.Status404NotFound,
                    "Not Found",
                    ex.Message));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error getting phrases for lesson {LessonId}", lessonId);
                return StatusCode(StatusCodes.Status500InternalServerError,
                    CreateProblemDetails(
                        StatusCodes.Status500InternalServerError,
                        "Server Error",
                        "An unexpected error occurred while processing your request"));
            }
        }

        [HttpGet("{lessonId:int}/quizzes")]
        [ProducesResponseType(typeof(List<QuizDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<List<QuizDto>>> GetLessonQuizzes(int lessonId)
        {
            try
            {
                return Ok(await _lessonService.GetLessonQuizzesAsync(lessonId));
            }
            catch (ArgumentException ex)
            {
                _logger.LogWarning(ex, "Invalid request for lesson quizzes: {LessonId}", lessonId);
                return BadRequest(CreateProblemDetails(
                    StatusCodes.Status400BadRequest,
                    "Invalid Request",
                    ex.Message));
            }
            catch (KeyNotFoundException ex)
            {
                _logger.LogWarning(ex, "No quizzes found for lesson: {LessonId}", lessonId);
                return NotFound(CreateProblemDetails(
                    StatusCodes.Status404NotFound,
                    "Not Found",
                    ex.Message));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error getting quizzes for lesson {LessonId}", lessonId);
                return StatusCode(StatusCodes.Status500InternalServerError,
                    CreateProblemDetails(
                        StatusCodes.Status500InternalServerError,
                        "Server Error",
                        "An unexpected error occurred while processing your request"));
            }
        }

        [HttpGet("{id:int}/detail")]
        [ProducesResponseType(typeof(LessonDetailDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<LessonDetailDto>> GetLessonDetail(int id)
        {
            try
            {
                return Ok(await _lessonService.GetLessonDetailAsync(id));
            }
            catch (ArgumentException ex)
            {
                _logger.LogWarning(ex, "Invalid request for lesson detail: {Id}", id);
                return BadRequest(CreateProblemDetails(
                    StatusCodes.Status400BadRequest,
                    "Invalid Request",
                    ex.Message));
            }
            catch (KeyNotFoundException ex)
            {
                _logger.LogWarning(ex, "Lesson detail not found: {Id}", id);
                return NotFound(CreateProblemDetails(
                    StatusCodes.Status404NotFound,
                    "Not Found",
                    ex.Message));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error getting lesson detail: {Id}", id);
                return StatusCode(StatusCodes.Status500InternalServerError,
                    CreateProblemDetails(
                        StatusCodes.Status500InternalServerError,
                        "Server Error",
                        "An unexpected error occurred while processing your request"));
            }
        }

        private ProblemDetails CreateProblemDetails(int statusCode, string title, string detail) =>
            new()
            {
                Status = statusCode,
                Title = title,
                Detail = detail,
                Instance = HttpContext.Request.Path
            };

        private ProblemDetails CreateProblemDetails(int statusCode, string title, Microsoft.AspNetCore.Mvc.ModelBinding.ModelStateDictionary modelState) =>
            new()
            {
                Status = statusCode,
                Title = title,
                Detail = "One or more validation errors occurred",
                Instance = HttpContext.Request.Path,
                Extensions = { ["errors"] = modelState.ToDictionary(
                    kvp => kvp.Key,
                    kvp => kvp.Value.Errors.Select(e => e.ErrorMessage).ToArray()) }
            };
    }
}