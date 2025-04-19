using Application.DtoModels.Pronunciation;
using Application.Services.Interfaces.IServices.Pronunciation;
using Microsoft.AspNetCore.Mvc;

namespace LangLearningAPI.Controllers.Pronunciation
{
    [ApiController]
    [Route("api/[controller]")]
    public class PronunciationController : ControllerBase
    {
        private readonly IPronunciationService _service;

        public PronunciationController(IPronunciationService service)
        {
            _service = service;
        }

        [HttpGet("categories")]
        public async Task<IActionResult> GetAllCategories()
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
        public async Task<IActionResult> GetCategory(int id)
        {
            try
            {
                var category = await _service.GetCategoryByIdAsync(id);
                return category == null ? NotFound() : Ok(category);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Server error: {ex.Message}");
            }
        }

        [HttpPost("categories")]
        public async Task<IActionResult> CreateCategory([FromBody] CreateCategoryDto dto)
        {
            try
            {
                var result = await _service.CreateCategoryAsync(dto);
                return result == null
                    ? BadRequest("Failed to create category.")
                    : CreatedAtAction(nameof(GetCategory), new { id = result.Id }, result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Server error: {ex.Message}");
            }
        }

        [HttpPatch("categories")]
        public async Task<IActionResult> UpdateCategory([FromBody] UpdateCategoryDto dto)
        {
            try
            {
                var result = await _service.UpdateCategoryAsync(dto);
                return result == null ? NotFound() : Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Server error: {ex.Message}");
            }
        }

        [HttpDelete("categories/{id}")]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            try
            {
                var result = await _service.DeleteCategoryAsync(id);
                return result == null ? NotFound() : Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Server error: {ex.Message}");
            }
        }

        [HttpGet("categories/{categoryId}/words")]
        public async Task<IActionResult> GetWordsByCategoryId(int categoryId)
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
        public async Task<IActionResult> AddWordToCategory([FromBody] CreateWordItemDto dto)
        {
            try
            {
                var result = await _service.AddWordToCategoryAsync(dto.CategoryId, dto);
                return result == null ? BadRequest("Failed to add word.") : Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Server error: {ex.Message}");
            }
        }

        [HttpPatch("words")]
        public async Task<IActionResult> UpdateWord([FromBody] UpdateWordItemDto dto)
        {
            try
            {
                var result = await _service.UpdateWordAsync(dto);
                return result == null ? NotFound() : Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Server error: {ex.Message}");
            }
        }

        [HttpDelete("words/{wordId}")]
        public async Task<IActionResult> DeleteWord(int wordId)
        {
            try
            {
                var result = await _service.DeleteWordAsync(wordId);
                return result == null ? NotFound() : Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Server error: {ex.Message}");
            }
        }
    }
}