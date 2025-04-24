using Application.DtoModels.Name.FemaleName;
using Application.Services.Interfaces.IServices.Name;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers.Name
{
    [ApiController]
    [Route("api/[controller]")]
    public class FemaleNameController : ControllerBase
    {
        private readonly IFemaleNameService _femaleNameService;
        private readonly ILogger<FemaleNameController> _logger;

        public FemaleNameController(IFemaleNameService femaleNameService, ILogger<FemaleNameController> logger)
        {
            _femaleNameService = femaleNameService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<FemaleNameDto>>> GetAllFemaleNamesAsync()
        {
            try
            {
                return Ok(await _femaleNameService.GetAllFemaleNamesAsync());
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to retrieve all female names.");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("{id}", Name = "GetFemaleNameById")]
        public async Task<ActionResult<FemaleNameDto>> GetFemaleNameByIdAsync(int id)
        {
            try
            {
                var result = await _femaleNameService.GetFemaleNameByIdAsync(id);
                if (result == null)
                {
                    _logger.LogWarning("Female name with ID {Id} not found.", id);
                    return NotFound();
                }

                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to retrieve female name with ID {Id}.", id);
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPost]
        public async Task<ActionResult<FemaleNameDto>> CreateFemaleNameAsync([FromBody] CreateFemaleNameDto dto)
        {
            try
            {
                var result = await _femaleNameService.CreateFemaleNameAsync(dto);
                if (result == null)
                {
                    _logger.LogWarning("Failed to create female name. EnglishNameId {Id} might not exist.", dto.EnglishNameId);
                    return BadRequest("Creation failed. Check if EnglishNameId exists.");
                }

                return CreatedAtRoute("GetFemaleNameById", new { id = result.Id }, result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to create female name.");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPut]
        public async Task<ActionResult<FemaleNameDto>> UpdateFemaleNameAsync([FromBody] UpdateFemaleNameDto dto)
        {
            try
            {
                var result = await _femaleNameService.UpdateFemaleNameAsync(dto.Id, dto);
                if (result == null)
                {
                    _logger.LogWarning("Failed to update female name. ID {Id} or EnglishNameId {EnglishNameId} might not exist.", dto.Id, dto.EnglishNameId);
                    return NotFound();
                }

                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to update female name with ID {Id}.", dto.Id);
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteFemaleNameAsync(int id)
        {
            try
            {
                var deleted = await _femaleNameService.DeleteFemaleNameAsync(id);
                if (!deleted)
                {
                    _logger.LogWarning("Failed to delete female name with ID {Id}. Not found.", id);
                    return NotFound();
                }

                return Ok(deleted);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to delete female name with ID {Id}.", id);
                return StatusCode(500, "Internal server error");
            }
        }
    }
}
