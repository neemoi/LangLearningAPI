using Application.DtoModels.MainQuestions;
using Application.Services.Interfaces.IServices.MainQuestions;
using Microsoft.AspNetCore.Mvc;

namespace LangLearningAPI.Controllers.MainQuestions
{
    [ApiController]
    [Route("api/[controller]")]
    public class MainQuestionsController : ControllerBase
    {
        private readonly IMainQuestionService _service;

        public MainQuestionsController(IMainQuestionService service)
        {
            _service = service;
        }

        [HttpGet("categories")]
        public async Task<IActionResult> GetAllCategoriesAsync()
        {
            try
            {
                return Ok(await _service.GetAllCategoriesAsync());
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Server error: {ex.Message}");
            }
        }

        [HttpGet("categories/{id}")]
        public async Task<IActionResult> GetCategoryByIdAsync(int id)
        {
            try
            {
                var result = await _service.GetCategoryByIdAsync(id);
                return result == null
                    ? NotFound($"Category with ID {id} not found.")
                    : Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Server error: {ex.Message}");
            }
        }

        [HttpPost("categories")]
        public async Task<IActionResult> CreateCategoryAsync([FromBody] CreateMainQuestionDto dto)
        {
            try
            {
                var result = await _service.CreateCategoryAsync(dto);
                return result == null
                    ? BadRequest("Failed to create the category.")
                    : CreatedAtAction(nameof(GetCategoryByIdAsync), new { id = result.Id }, result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Server error: {ex.Message}");
            }
        }

        [HttpPatch]
        public async Task<IActionResult> UpdateCategoryAsync([FromBody] UpdateMainQuestionDto dto)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var updatedCategory = await _service.UpdateCategoryAsync(dto.Id, dto);

                if (updatedCategory == null)
                    return NotFound($"Category with ID {dto.Id} not found.");

                return Ok(updatedCategory);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Server error: {ex.Message}");
            }
        }

        [HttpDelete("categories/{id}")]
        public async Task<IActionResult> DeleteCategoryAsync(int id)
        {
            try
            {
                var success = await _service.DeleteCategoryAsync(id);
                return success
                    ? Ok($"Category with ID {id} deleted successfully.")
                    : NotFound($"Category with ID {id} not found.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Server error: {ex.Message}");
            }
        }

        [HttpGet("categories/{categoryId}/words")]
        public async Task<IActionResult> GetWordsByCategoryIdAsync(int categoryId)
        {
            try
            {
                return Ok(await _service.GetWordsByCategoryIdAsync(categoryId));
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Server error: {ex.Message}");
            }
        }

        [HttpPost("categories/words")]
        public async Task<IActionResult> AddWordToMainQuestionAsync([FromBody] CreateMainQuestionWordDto dto)
        {
            try
            {
                return Ok(await _service.AddWordToMainQuestionAsync(dto));
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Server error: {ex.Message}");
            }
        }

        [HttpPatch("words")]
        public async Task<IActionResult> UpdateWordAsync([FromBody] UpdateMainQuestionWordDto dto)
        {
            try
            {
                var success = await _service.UpdateWordAsync(dto.Id, dto);
                return success
                    ? Ok($"Word with ID {dto.Id} updated successfully.")
                    : NotFound($"Word with ID {dto.Id} not found.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Server error: {ex.Message}");
            }
        }

        [HttpDelete("words/{wordId}")]
        public async Task<IActionResult> DeleteWordAsync(int wordId)
        {
            try
            {
                var success = await _service.DeleteWordAsync(wordId);
                return success
                    ? Ok($"Word with ID {wordId} deleted successfully.")
                    : NotFound($"Word with ID {wordId} not found.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Server error: {ex.Message}");
            }
        }
    }
}