using Application.DtoModels.Name.MaleName;
using Application.Services.Interfaces.IServices.Name;
using Microsoft.AspNetCore.Mvc;

namespace LangLearningAPI.Controllers.Name
{
    [ApiController]
    [Route("api/[controller]")]
    public class MaleNameController : ControllerBase
    {
        private readonly IMaleNameService _maleNameService;
        private readonly ILogger<MaleNameController> _logger;

        public MaleNameController(IMaleNameService maleNameService, ILogger<MaleNameController> logger)
        {
            _maleNameService = maleNameService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<MaleNameDto>>> GetAllMaleNamesAsync()
        {
            try
            {
                return Ok(await _maleNameService.GetAllMaleNamesAsync());
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to retrieve all male names.");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("{id}", Name = "GetMaleNameByIdAsync")]

        public async Task<ActionResult<MaleNameDto>> GetMaleNameByIdAsync(int id)
        {
            try
            {
                var result = await _maleNameService.GetMaleNameByIdAsync(id);
                if (result == null)
                {
                    _logger.LogWarning("Male name with ID {Id} not found.", id);
                    return NotFound();
                }

                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to retrieve male name with ID {Id}.", id);
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPost]
        public async Task<ActionResult<MaleNameDto>> CreateMaleNameAsync([FromBody] CreateMaleNameDto dto)
        {
            try
            {
                var result = await _maleNameService.CreateMaleNameAsync(dto);
                if (result == null)
                {
                    _logger.LogWarning("Failed to create male name. EnglishNameId {Id} might not exist.", dto.EnglishNameId);
                    return BadRequest("Creation failed. Check if EnglishNameId exists.");
                }

                return CreatedAtRoute("GetMaleNameByIdAsync", new { id = result.Id }, result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to create male name.");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPut]
        public async Task<ActionResult<MaleNameDto>> UpdateMaleNameAsync([FromBody] UpdateMaleNameDto dto)
        {
            try
            {
                var result = await _maleNameService.UpdateMaleNameAsync(dto.Id, dto);
                if (result == null)
                {
                    _logger.LogWarning("Failed to update male name. ID {Id} or EnglishNameId {EnglishNameId} might not exist.", dto.Id, dto.EnglishNameId);
                    return NotFound();
                }

                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to update male name with ID {Id}.", dto.Id);
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMaleNameAsync(int id)
        {
            try
            {
                var deleted = await _maleNameService.DeleteMaleNameAsync(id);
                if (!deleted)
                {
                    _logger.LogWarning("Failed to delete male name with ID {Id}. Not found.", id);
                    return NotFound();
                }

                return Ok(deleted);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to delete male name with ID {Id}.", id);
                return StatusCode(500, "Internal server error");
            }
        }
    }
}
