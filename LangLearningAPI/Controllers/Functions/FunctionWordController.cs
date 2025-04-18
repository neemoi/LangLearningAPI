using Application.DtoModels.Functions;
using Application.Services.Interfaces.IServices.Functions;
using Microsoft.AspNetCore.Mvc;

namespace LangLearningAPI.Controllers.Functions
{
    [ApiController]
    [Route("api/[controller]")]
    public class LanguageController : ControllerBase
    {
        private readonly IFunctionWordService _functionWordService;
        private readonly IPartOfSpeechService _partOfSpeechService;
        private readonly ILogger<LanguageController> _logger;

        public LanguageController(IFunctionWordService functionWordService, IPartOfSpeechService partOfSpeechService, ILogger<LanguageController> logger)
        {
            _functionWordService = functionWordService;
            _partOfSpeechService = partOfSpeechService;
            _logger = logger;
        }

        [HttpGet("function-word/{id}", Name = "GetFunctionWordById")]
        public async Task<IActionResult> GetFunctionWordByIdAsync(int id)
        {
            try
            {
                var result = await _functionWordService.GetFunctionWordByIdAsync(id);
                if (result == null)
                    return NotFound($"FunctionWord with ID {id} was not found");

                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while retrieving FunctionWord with ID {Id}", id);
                throw;
            }
        }

        [HttpGet("function-word")]
        public async Task<IActionResult> GetAllFunctionWordAsync()
        {
            try
            {
                return Ok(await _functionWordService.GetAllFunctionWordAsync());
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while retrieving all FunctionWords");
                throw;
            }
        }

        [HttpPost("function-word")]
        public async Task<IActionResult> AddFunctionWordAsync([FromBody] CreateFunctionWordDto model)
        {
            try
            {
                var createdDto = await _functionWordService.AddFunctionWordAsync(model);
                return CreatedAtRoute("GetFunctionWordById", new { id = createdDto.Id }, createdDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while adding FunctionWord");
                throw;
            }
        }

        [HttpPut("function-word")]
        public async Task<IActionResult> UpdateFunctionWordAsync([FromBody] FunctionWordUpdateDto dto)
        {
            try
            {
                return Ok(await _functionWordService.UpdateFunctionWordAsync(dto));
            }
            catch (KeyNotFoundException)
            {
                return NotFound($"FunctionWord с ID {dto.Id} не найден");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating Function Word with ID {Id}", dto.Id);
                throw;
            }
        }

        [HttpDelete("function-word/{id}")]
        public async Task<IActionResult> DeleteFunctionWordAsync(int id)
        {
            try
            {
                var deleted = await _functionWordService.DeleteFunctionWordAsync(id);
                if (!deleted)
                    return NotFound($"FunctionWord with ID {id} was not found");

                return Ok(deleted);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while deleting FunctionWord with ID {Id}", id);
                throw;
            }
        }
    }
}