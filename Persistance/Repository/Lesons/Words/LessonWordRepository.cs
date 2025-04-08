using Application.Services.Interfaces.IRepository.Lesons;
using Domain.Models;
using Infrastructure.Data;
using LangLearningAPI.Exceptions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Net;

namespace Persistance.Repository.Lesons.Words
{
    public class LessonWordRepository : ILessonWordRepository
    {
        private readonly LanguageLearningDbContext _context;
        private readonly ILogger<LessonWordRepository> _logger;

        public LessonWordRepository(LanguageLearningDbContext context, ILogger<LessonWordRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<IEnumerable<LessonWord>> GetAllWordsAsync()
        {
            try
            {
                return await _context.LessonWords
                    .Include(lw => lw.Lesson)
                    .AsNoTracking()
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while fetching all lesson words");
                throw new ApiException(
                    statusCode: (int)HttpStatusCode.InternalServerError,
                    message: "Failed to retrieve lesson words",
                    errorCode: "LESSON_WORDS_GET_ALL_ERROR");
            }
        }

        public async Task<LessonWord?> GetWordByIdAsync(int id)
        {
            try
            {
                ValidateId(id);

                return await _context.LessonWords
                    .Include(lw => lw.Lesson)
                    .AsNoTracking()
                    .FirstOrDefaultAsync(lw => lw.Id == id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error fetching lesson word with ID {id}");
                throw new ApiException(
                    statusCode: (int)HttpStatusCode.InternalServerError,
                    message: $"Failed to retrieve lesson word with ID {id}",
                    errorCode: "LESSON_WORD_GET_ERROR");
            }
        }

        public async Task<LessonWord> CreateWordsAsync(LessonWord entity)
        {
            try
            {
                ValidateEntity(entity);
                await ValidateDuplicateWord(entity);


                var exists = await _context.LessonWords
                    .AnyAsync(lw => lw.LessonId == entity.LessonId &&
                                   lw.Name == entity.Name);

                if (exists)
                    throw new InvalidOperationException("Word already exists in this lesson");

                await _context.LessonWords.AddAsync(entity);
                await _context.SaveChangesAsync();

                return entity;
            }
            catch (Exception ex) when (ex is ValidationException or InvalidOperationException)
            {
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding lesson word");
                throw new ApiException(
                    statusCode: (int)HttpStatusCode.InternalServerError,
                    message: "Failed to create lesson word",
                    errorCode: "LESSON_WORD_CREATE_ERROR");
            }
        }

        public async Task<LessonWord> UpdateWordsAsync(LessonWord word)
        {
            try
            {
                ValidateEntity(word);

                var existing = await _context.LessonWords.FindAsync(word.Id)
                    ?? throw new NotFoundException($"LessonWord with ID {word.Id} not found");

                _context.Entry(existing).CurrentValues.SetValues(word);
                await _context.SaveChangesAsync();

                return existing;
            }
            catch (NotFoundException)
            {
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error updating lesson word with ID {word?.Id}");
                throw new ApiException(
                    statusCode: (int)HttpStatusCode.InternalServerError,
                    message: $"Failed to update lesson word with ID {word?.Id}",
                    errorCode: "LESSON_WORD_UPDATE_ERROR");
            }
        }

        public async Task<LessonWord> DeleteWordsAsync(int id)
        {
            try
            {
                ValidateId(id);

                var word = await _context.LessonWords.FindAsync(id)
                    ?? throw new NotFoundException($"LessonWord with ID {id} not found");

                _context.LessonWords.Remove(word);
                await _context.SaveChangesAsync();

                return word;
            }
            catch (NotFoundException)
            {
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error deleting lesson word with ID {id}");
                throw new ApiException(
                    statusCode: (int)HttpStatusCode.InternalServerError,
                    message: $"Failed to delete lesson word with ID {id}",
                    errorCode: "LESSON_WORD_DELETE_ERROR");
            }
        }

        public async Task<bool> ExistsAsync(int id)
        {
            try
            {
                ValidateId(id);
                return await _context.LessonWords.AnyAsync(lw => lw.Id == id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error checking existence of lesson word with ID {id}");
                throw new ApiException(
                    statusCode: (int)HttpStatusCode.InternalServerError,
                    message: $"Failed to check existence of lesson word with ID {id}",
                    errorCode: "LESSON_WORD_EXISTS_ERROR");
            }
        }

        #region Private Validation Methods

        private void ValidateId(int id)
        {
            if (id <= 0)
            {
                throw new ValidationException(
                    message: "ID must be greater than zero",
                    errorCode: "INVALID_ID",
                    errors: new Dictionary<string, string[]>
                    {
                        { "id", new[] { "ID must be greater than 0" } }
                    });
            }
        }

        private void ValidateEntity(LessonWord entity)
        {
            if (entity == null)
            {
                throw new ValidationException(
                    message: "Entity cannot be null",
                    errorCode: "NULL_ENTITY");
            }

            var errors = new Dictionary<string, string[]>();

            if (string.IsNullOrWhiteSpace(entity.Name))
            {
                errors.Add(nameof(entity.Name), new[] { "Name is required" });
            }

            if (string.IsNullOrWhiteSpace(entity.Translation))
            {
                errors.Add(nameof(entity.Translation), new[] { "Translation is required" });
            }

            if (entity.LessonId <= 0)
            {
                errors.Add(nameof(entity.LessonId), new[] { "Invalid Lesson ID" });
            }

            if (errors.Any())
            {
                throw new ValidationException(
                    message: "Validation failed for LessonWord",
                    errorCode: "VALIDATION_ERROR",
                    errors: errors);
            }
        }

        private async Task ValidateDuplicateWord(LessonWord entity)
        {
            bool exists = await _context.LessonWords
                .AnyAsync(lw => lw.LessonId == entity.LessonId &&
                               lw.Name == entity.Name);

            if (exists)
            {
                throw new ConflictException(
                    message: $"Word '{entity.Name}' already exists in lesson {entity.LessonId}",
                    errorCode: "DUPLICATE_WORD");
            }
        }

        #endregion
    }
}