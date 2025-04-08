using Application.DtoModels.Lessons.Words;
using Application.Services.Interfaces.IServices.Lesons;
using LangLearningAPI.Exceptions;
using Microsoft.AspNetCore.Mvc;

namespace LangLearningAPI.Controllers.Lessons
{
    [Route("api/words")]
    [ApiController]
    public class LessonWordController : ControllerBase
    {
        private readonly ILessonWordService _lessonWordService;

        public LessonWordController(ILessonWordService lessonWordService)
        {
            _lessonWordService = lessonWordService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllWords()
        {
            try
            {
                return Ok(await _lessonWordService.GetAllWordsAsync());
            }
            catch (Exception)
            {
                throw; 
            }
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetWordById(int id)
        {
            try
            {
                var word = await _lessonWordService.GetWordByIdAsync(id);
                return word != null ? Ok(word) : NotFound();
            }
            catch (NotFoundException)
            {
                throw; 
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateWord([FromBody] CreateLessonWordDto dto)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var createdWord = await _lessonWordService.CreateWordAsync(dto);
                return CreatedAtAction(nameof(GetWordById), new { id = createdWord.Id }, createdWord);
            }
            catch (ValidationException)
            {
                throw;
            }
            catch (ConflictException)
            {
                throw;
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpPatch("{id:int}")]
        public async Task<IActionResult> UpdateWord(int id, [FromBody] UpdateLessonWordDto dto)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                return Ok(await _lessonWordService.UpdatePartialWordsAsync(id, dto));
            }
            catch (NotFoundException)
            {
                throw;
            }
            catch (ValidationException)
            {
                throw;
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteWord(int id)
        {
            try
            {
                return Ok(await _lessonWordService.DeleteWordsAsync(id));
            }
            catch (NotFoundException)
            {
                throw;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}