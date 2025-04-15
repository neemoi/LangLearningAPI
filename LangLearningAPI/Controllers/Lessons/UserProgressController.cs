using Application.DtoModels.Lessons.Progress;
using Application.Services.Interfaces.IServices.Lesons;
using LangLearningAPI.Exceptions;
using Microsoft.AspNetCore.Mvc;

namespace LangLearningAPI.Controllers.Lessons
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserProgressController : ControllerBase
    {
        private readonly IUserProgressService _userProgressService;
        private readonly ILogger<UserProgressController> _logger;

        public UserProgressController(IUserProgressService userProgressService, ILogger<UserProgressController> logger)
        {
            _userProgressService = userProgressService;
            _logger = logger;
        }

        [HttpPost]
        public async Task<IActionResult> AddUserProgress([FromBody] UserProgressCreateUpdateDto progressDto)
        {
            try
            {
                var result = await _userProgressService.AddUserProgressAsync(progressDto);
                return Ok(new
                {
                    Message = "User progress added successfully",
                    Progress = result
                });
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogWarning(ex, "Progress already exists");
                return Conflict(new { Message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding user progress");
                return BadRequest(new { Message = "Failed to add user progress", Error = ex.Message });
            }
        }

        [HttpPut]
        public async Task<IActionResult> UpdateUserProgress([FromBody] UserProgressCreateUpdateDto progressDto)
        {
            try
            {
                var result = await _userProgressService.UpdateUserProgressAsync(progressDto);
                return Ok(new
                {
                    Message = "User progress updated successfully",
                    Progress = result
                });
            }
            catch (NotFoundException ex)
            {
                _logger.LogWarning(ex, "Progress not found for update");
                return NotFound(new { Message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating user progress");
                return StatusCode(500, new { Message = "Internal server error", Error = ex.Message });
            }
        }

        [HttpGet("detailed/{userId}/{lessonId}")]
        public async Task<IActionResult> GetFullProgress(string userId, int lessonId)
        {
            try
            {
                return Ok(await _userProgressService.GetFullProgressAsync(userId, lessonId));
            }
            catch (NotFoundException ex)
            {
                _logger.LogWarning(ex, "User progress not found");
                return NotFound(new { Message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving detailed progress");
                return StatusCode(500, new { Message = "Internal server error", Error = ex.Message });
            }
        }

        [HttpGet("word-stats/{userId}/{lessonId}")]
        public async Task<IActionResult> GetWordStats(string userId, int lessonId)
        {
            try
            {
                return Ok(await _userProgressService.GetWordStatsAsync(userId, lessonId));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting word stats");
                return StatusCode(500, new { Message = "Internal server error", Error = ex.Message });
            }
        }

        [HttpPost("word-progress")]
        public async Task<IActionResult> AddWordProgress([FromBody] UserWordProgressCreateDto progressDto)
        {
            try
            {
                var result = await _userProgressService.AddWordProgressAsync(progressDto);
                return Ok(new
                {
                    Message = "Word progress added successfully",
                    Progress = result
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding word progress");
                return BadRequest(new { Message = "Failed to add word progress", Error = ex.Message });
            }
        }

        [HttpPut("word-progress/{id}")]
        public async Task<IActionResult> UpdateWordProgress(int id, [FromBody] UserWordProgressCreateDto progressDto)
        {
            try
            {
                var result = await _userProgressService.UpdateWordProgressAsync(id, progressDto);
                return Ok(new
                {
                    Message = "Word progress updated successfully",
                    Progress = result
                });
            }
            catch (NotFoundException ex)
            {
                _logger.LogWarning(ex, "Word progress not found");
                return NotFound(new { Message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating word progress");
                return StatusCode(500, new { Message = "Internal server error", Error = ex.Message });
            }
        }

        [HttpGet("word-progress/{id}")]
        public async Task<IActionResult> GetWordProgress(int id)
        {
            try
            {
                return Ok(await _userProgressService.GetWordProgressAsync(id));
            }
            catch (NotFoundException ex)
            {
                _logger.LogWarning(ex, "Word progress not found");
                return NotFound(new { Message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting word progress");
                return StatusCode(500, new { Message = "Internal server error", Error = ex.Message });
            }
        }

        [HttpGet("word-progress/user/{userId}/lesson/{lessonId}")]
        public async Task<IActionResult> GetWordProgressesByUserAndLesson(string userId, int lessonId)
        {
            try
            {
                return Ok(await _userProgressService.GetWordProgressesByUserAndLessonAsync(userId, lessonId));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting word progresses");
                return StatusCode(500, new { Message = "Internal server error", Error = ex.Message });
            }
        }
    }
}