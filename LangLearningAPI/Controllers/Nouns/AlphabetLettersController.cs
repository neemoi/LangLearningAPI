using Microsoft.AspNetCore.Mvc;
using Application.DtoModels.Nouns;
using Application.Services.Interfaces.IServices.Nouns;
using Domain.Models;

namespace WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AlphabetLetterController : ControllerBase
    {
        private readonly IAlphabetLetterService _letterService;
        private readonly INounWordService _nounWordService;
        private readonly ILogger<AlphabetLetterController> _logger;

        public AlphabetLetterController(IAlphabetLetterService letterService, INounWordService nounWordService,
            ILogger<AlphabetLetterController> logger)
        {
            _letterService = letterService;
            _nounWordService = nounWordService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<AlphabetLetterDto>>> GetAllLettersAsync()
        {
            try
            {
                return Ok(await _letterService.GetAllLettersAsync());
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving all alphabet letters");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("{id}", Name = "GetLetterByIdAsync")]
        public async Task<ActionResult<AlphabetLetterDto>> GetLetterByIdAsync(int id)
        {
            try
            {
                var letter = await _letterService.GetLetterByIdAsync(id);
                if (letter == null)
                    return NotFound();

                return Ok(letter);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error retrieving alphabet letter with id {id}");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPost]
        public async Task<ActionResult<AlphabetLetterDto>> AddLetterAsync([FromBody] CreateAlphabetLetterDto letterDto)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var createdLetter = await _letterService.AddLetterAsync(letterDto);
                var locationUrl = Url.Action(nameof(GetLetterByIdAsync), new { id = createdLetter.Id });

                return Created(locationUrl ?? string.Empty, createdLetter);
            }
            catch (ArgumentException ex)
            {
                _logger.LogWarning(ex, "Validation error while adding letter");
                return BadRequest(new { error = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating alphabet letter");
                return StatusCode(500, new { error = "Internal server error", details = ex.Message });
            }
        }

        [HttpPut]
        public async Task<ActionResult> UpdateLetterAsync([FromBody] UpdateAlphabetLetterDto letterDto)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                return Ok(await _letterService.UpdateLetterAsync(letterDto));
            }
            catch (ArgumentException ex)
            {
                _logger.LogWarning(ex, "Validation error while updating letter");
                return BadRequest(new { error = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error updating alphabet letter with id {letterDto.Id}");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteLetterAsync(int id)
        {
            try
            {
                var deleted = await _letterService.DeleteLetterAsync(id);
                if (!deleted)
                    return NotFound();

                return Ok(deleted);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error deleting alphabet letter with id {id}");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("words")]
        public async Task<ActionResult<IEnumerable<NounWord>>> GetAllNounWords()
        {
            try
            {
                return Ok(await _nounWordService.GetAllWordsAsync());
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving all noun words");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("words/{id}")]
        public async Task<ActionResult<NounWord>> GetNounWordById(int id)
        {
            try
            {
                var word = await _nounWordService.GetWordByIdAsync(id);
                if (word == null)
                    return NotFound();

                return Ok(word);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error retrieving noun word with id {id}");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("letters/{letterId}/words")]
        public async Task<ActionResult<IEnumerable<NounWord>>> GetWordsByLetterId(int letterId)
        {
            try
            {
                return Ok(await _nounWordService.GetWordsByLetterIdAsync(letterId));
            }
            catch (KeyNotFoundException ex)
            {
                _logger.LogWarning(ex, "No noun words found for letter id");
                return NotFound(new { error = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error retrieving noun words for letter id {letterId}");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPost("words")]
        public async Task<ActionResult<NounWord>> AddNounWord([FromBody] NounWordDto wordDto)
        {
            try
            {
                var createdWord = await _nounWordService.AddWordAsync(wordDto);
                return CreatedAtAction(nameof(GetNounWordById), new { id = createdWord.Id }, createdWord);
            }
            catch (ArgumentException ex)
            {
                _logger.LogWarning(ex, "Validation error while adding noun word");
                return BadRequest(new { error = ex.Message });
            }
            catch (KeyNotFoundException ex)
            {
                _logger.LogWarning(ex, "Letter not found when adding noun word");
                return NotFound(new { error = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating noun word");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPut("words")]
        public async Task<ActionResult<NounWord>> UpdateNounWord([FromBody] UpdateNounWordDto wordDto)
        {
            try
            {
                return Ok(await _nounWordService.UpdateWordAsync(wordDto));
            }
            catch (ArgumentException ex)
            {
                _logger.LogWarning(ex, "Validation error while updating noun word");
                return BadRequest(new { error = ex.Message });
            }
            catch (KeyNotFoundException ex)
            {
                _logger.LogWarning(ex, "Word or letter not found");
                return NotFound(new { error = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating noun word");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpDelete("words/{id}")]
        public async Task<ActionResult> DeleteNounWord(int id)
        {
            try
            {
                var deleted = await _nounWordService.DeleteWordAsync(id);
                if (!deleted)
                    return NotFound();

                return Ok(deleted);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error deleting noun word with id {id}");
                return StatusCode(500, "Internal server error");
            }
        }
    }
}
