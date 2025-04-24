using Application.DtoModels.Name.EnglishName;
using Application.Services.Interfaces.IServices.Name;
using Microsoft.AspNetCore.Mvc;

namespace LangLearningAPI.Controllers.Name
{
    [ApiController]
    [Route("api/[controller]")]
    public class NameController : ControllerBase
    {
        private readonly INameService _nameService;

        public NameController(INameService nameService)
        {
            _nameService = nameService;
        }

        [HttpGet("englishnames")]
        public async Task<ActionResult<IEnumerable<EnglishNameDto>>> GetAllEnglishNames()
        {
            try
            {
                var result = await _nameService.GetAllEnglishNamesAsync();
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An error occurred while retrieving data.");
            }
        }

        [HttpGet("englishnames/{id}", Name = "GetEnglishNameById")]
        public async Task<ActionResult<EnglishNameDto>> GetEnglishNameById(int id)
        {
            try
            {
                var result = await _nameService.GetEnglishNameByIdAsync(id);
                if (result == null)
                    return NotFound();
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An error occurred while retrieving data.");
            }
        }

        [HttpPost("englishnames")]
        public async Task<ActionResult<EnglishNameDto>> CreateEnglishName([FromBody] CreateEnglishNameDto dto)
        {
            try
            {
                var result = await _nameService.CreateEnglishNameAsync(dto);
                if (result == null)
                    return BadRequest("Failed to create English name.");

                return CreatedAtRoute("GetEnglishNameById", new { id = result.Id }, result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An error occurred while creating the entity.");
            }
        }

        [HttpPut("englishnames")]
        public async Task<IActionResult> UpdateEnglishName([FromBody] UpdateEnglishNameDto dto)
        {
            try
            {
                return Ok(await _nameService.UpdateEnglishNameAsync(dto.Id, dto));
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An error occurred while updating the entity.");
            }
        }

        [HttpDelete("englishnames/{id}")]
        public async Task<IActionResult> DeleteEnglishName(int id)
        {
            try
            {
                return Ok(await _nameService.DeleteEnglishNameAsync(id));
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An error occurred while deleting the entity.");
            }
        }
    }
}
