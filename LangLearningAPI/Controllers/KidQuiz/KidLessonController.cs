using Application.DtoModels.KidQuiz.Lessons;
using Application.Services.Implementations.KidQuiz;
using Microsoft.AspNetCore.Mvc;

namespace LangLearningAPI.Controllers.KidQuiz
{
    [ApiController]
    [Route("api/kid-lessons")]
    public class KidLessonController : ControllerBase
    {
        private readonly KidLessonService _kidLessonService;
        private readonly ILogger<KidLessonController> _logger;

        public KidLessonController(KidLessonService kidLessonService, ILogger<KidLessonController> logger)
        {
            _kidLessonService = kidLessonService;
            _logger = logger;
        }

        [HttpPost]
        public async Task<ActionResult<KidLessonDto>> CreateLessonAsync([FromBody] CreateKidLessonDto dto)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var createdLesson = await _kidLessonService.CreateLessonAsync(dto);
                if (createdLesson == null)
                    return Problem("Failed to create lesson");

                return Created(
                    uri: $"/api/kid-lessons/{createdLesson.Id}",
                    value: createdLesson);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating lesson");
                return Problem("Internal server error");
            }
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<KidLessonDto>> GetLessonByIdAsync(int id)
        {
            try
            {
                var lesson = await _kidLessonService.GetLessonByIdAsync(id);
                return lesson == null ? NotFound() : Ok(lesson);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error getting lesson {id}");
                return Problem("Internal server error");
            }
        }

        [HttpGet]
        public async Task<ActionResult<List<KidLessonDto>>> GetAllLessonsAsync()
        {
            try
            {
                return Ok(await _kidLessonService.GetAllLessonsAsync());
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting all lessons");
                return Problem("Internal server error");
            }
        }

        [HttpPut]
        public async Task<IActionResult> UpdateLessonAsync([FromBody] UpdateKidLessonDto dto)
        {
            try
            {
                var result = await _kidLessonService.UpdateLessonAsync(dto.Id, dto);
                return result == null ? NotFound() : Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error updating lesson {dto.Id}");
                return Problem("Internal server error");
            }
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteLessonAsync(int id)
        {
            try
            {
                return Ok(await _kidLessonService.DeleteLessonAsync(id));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error occurred while deleting KidLesson with ID {id}.");
                return StatusCode(500, "Internal server error.");
            }
        }
    }
}