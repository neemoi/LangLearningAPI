using Application.DtoModels.Functions;
using Application.Services.Interfaces.IServices.Functions;
using Microsoft.AspNetCore.Mvc;

namespace LangLearningAPI.Controllers.Functions
{
    [Route("api/[controller]")]
    [ApiController]
    public class PartOfSpeechController : ControllerBase
    {
        private readonly IPartOfSpeechService _service;
        private readonly ILogger<PartOfSpeechController> _logger;

        public PartOfSpeechController(IPartOfSpeechService service, ILogger<PartOfSpeechController> logger)
        {
            _service = service;
            _logger = logger;
        }

        [HttpGet("{id}", Name = "GetPartOfSpeechById")]
        public async Task<IActionResult> GetPartOfSpeechByIdAsync(int id)
        {
            try
            {
                var result = await _service.GetPartOfSpeechByIdAsync(id);
                if (result == null)
                    return NotFound($"PartOfSpeech with ID {id} was not found");

                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving PartOfSpeech with ID {Id}", id);
                throw;
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetAllPartOfSpeechAsync()
        {
            try
            {
                return Ok(await _service.GetAllPartOfSpeechAsync());
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving all PartOfSpeech");
                throw;
            }
        }

        [HttpPost]
        public async Task<IActionResult> AddPartOfSpeechAsync([FromBody] CreatePartOfSpeechDto dto)
        {
            try
            {
                var created = await _service.AddPartOfSpeechAsync(dto);
                return CreatedAtAction("GetPartOfSpeechById", new { id = created.Id }, created); 
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding PartOfSpeech");
                throw;
            }
        }

        [HttpPut]
        public async Task<IActionResult> UpdatePartOfSpeechAsync([FromBody] FunctionWordUpdateDto dto)
        {
            try
            {
                return Ok(await _service.UpdatePartOfSpeechAsync(dto));
            }
            catch (KeyNotFoundException)
            {
                return NotFound($"PartOfSpeech with ID {dto.Id} was not found");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating PartOfSpeech with ID {Id}", dto.Id);
                throw;
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePartOfSpeechAsync(int id)
        {
            try
            {
                var deleted = await _service.DeletePartOfSpeechAsync(id);
                if (!deleted)
                    return NotFound($"PartOfSpeech with ID {id} was not found");

                return Ok(deleted);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting PartOfSpeech with ID {Id}", id);
                throw;
            }
        }
    }
}
