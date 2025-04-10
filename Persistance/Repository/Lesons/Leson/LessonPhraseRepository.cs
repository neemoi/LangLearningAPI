using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using AutoMapper;
using Infrastructure.Data;
using Application.Services.Interfaces.IRepository.Lessons;
using LangLearningAPI.Exceptions;
using Application.DtoModels.Lessons.Phrasees;

namespace Persistance.Repository.Lessons.Lesson
{
    public class LessonPhraseRepository : ILessonPhraseRepository
    {
        private readonly LanguageLearningDbContext _context;
        private readonly ILogger<LessonPhraseRepository> _logger;
        private readonly IMapper _mapper;

        public LessonPhraseRepository(LanguageLearningDbContext context, ILogger<LessonPhraseRepository> logger, IMapper mapper)
        {
            _context = context;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task<LessonPhrase> CreatePhraseAsync(LessonPhrase entity)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                _logger.LogInformation("Creating new phrase");

                if (entity == null)
                    throw new ArgumentNullException(nameof(entity), "Phrase entity cannot be null");

                if (entity.LessonId <= 0)
                    throw new ArgumentException("Invalid LessonId", nameof(entity.LessonId));

                bool lessonExists;
                try
                {
                    lessonExists = await _context.Lessons.AnyAsync(l => l.Id == entity.LessonId);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error checking existence of lesson with ID: {LessonId}", entity.LessonId);
                    throw new RepositoryException($"Error checking existence of lesson with ID {entity.LessonId}", "DB_QUERY_ERROR", ex);
                }

                if (!lessonExists)
                    throw new NotFoundException($"Lesson with ID {entity.LessonId} not found", "LESSON_NOT_FOUND");

                bool phraseExists = await _context.LessonPhrases.AnyAsync(p =>
                    p.LessonId == entity.LessonId &&
                    p.PhraseText == entity.PhraseText &&
                    p.Translation == entity.Translation &&
                    p.ImageUrl == entity.ImageUrl
                );

                if (phraseExists)
                    throw new ConflictException("A phrase with the same text, translation, and image already exists in this lesson.", "PHRASE_ALREADY_EXISTS");

                await _context.LessonPhrases.AddAsync(entity);
                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                _logger.LogInformation("Successfully created phrase with ID: {PhraseId}", entity.Id);

                return entity;
            }
            catch (NotFoundException)
            {
                await transaction.RollbackAsync();
                throw;
            }
            catch (ConflictException)
            {
                await transaction.RollbackAsync();
                throw;
            }
            catch (DbUpdateException ex)
            {
                await transaction.RollbackAsync();
                _logger.LogError(ex, "Database error while creating phrase");
                throw new RepositoryException("Database error occurred while creating phrase", "DB_SAVE_ERROR", ex);
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                _logger.LogError(ex, "Unexpected error creating phrase");
                throw new RepositoryException("An unexpected error occurred while creating phrase", "UNEXPECTED_ERROR", ex);
            }
        }

        public async Task<LessonPhrase> DeletePhraseAsync(int id)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                _logger.LogInformation("Deleting phrase with ID: {PhraseId}", id);

                var phrase = await _context.LessonPhrases
                    .FirstOrDefaultAsync(p => p.Id == id)
                    ?? throw new NotFoundException($"Phrase with ID {id} not found", "PHRASE_NOT_FOUND");

                _context.LessonPhrases.Remove(phrase);
                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                _logger.LogInformation("Successfully deleted phrase with ID: {PhraseId}", id);

                return phrase;
            }
            catch (NotFoundException ex)
            {
                await transaction.RollbackAsync();
                _logger.LogWarning(ex, "Phrase not found for deletion: {PhraseId}", id);
                throw;
            }
            catch (DbUpdateException ex)
            {
                await transaction.RollbackAsync();
                _logger.LogError(ex, "Database error while deleting phrase with ID: {PhraseId}", id);
                throw new RepositoryException($"Database error occurred while deleting phrase with ID {id}", "DB_DELETE_ERROR", ex);
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                _logger.LogError(ex, "Unexpected error deleting phrase with ID: {PhraseId}", id);
                throw new RepositoryException($"An unexpected error occurred while deleting phrase with ID {id}", "UNEXPECTED_ERROR", ex);
            }
        }

        public async Task<IEnumerable<LessonPhrase>> GetAllPhrasesAsync()
        {
            try
            {
                _logger.LogInformation("Getting all phrases");

                return await _context.LessonPhrases
                    .AsNoTracking()
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting all phrases");
                throw new RepositoryException("Error occurred while getting all phrases", "DB_QUERY_ERROR", ex);
            }
        }

        public async Task<LessonPhrase> GetPhraseByIdAsync(int id)
        {
            try
            {
                _logger.LogInformation("Getting phrase by ID: {PhraseId}", id);

                if (id <= 0)
                    throw new ArgumentException("Invalid phrase ID", nameof(id));

                var phrase = await _context.LessonPhrases.FindAsync(id);

                if (phrase == null)
                {
                    _logger.LogWarning("Phrase with ID {PhraseId} not found", id);
                    throw new NotFoundException($"Phrase with ID {id} not found", "PHRASE_NOT_FOUND");
                }

                return phrase;
            }
            catch (NotFoundException ex)
            {
                _logger.LogWarning(ex, "Phrase not found: {PhraseId}", id);
                throw;
            }
            catch (ArgumentException ex)
            {
                _logger.LogWarning(ex, "Invalid phrase ID: {PhraseId}", id);
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting phrase with ID: {PhraseId}", id);
                throw new RepositoryException($"Error occurred while getting phrase with ID {id}", "DB_QUERY_ERROR", ex);
            }
        }

        public async Task<LessonPhrase> UpdatePhraseAsync(UpdateLessonPhraseDto dto)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                _logger.LogInformation("Updating phrase with ID: {PhraseId}", dto.PhraseId);

                var existingLesson = await _context.Lessons
                    .FirstOrDefaultAsync(p => p.Id == dto.LessonId)
                    ?? throw new NotFoundException($"Lesson with ID {dto.LessonId} not found", "PHRASE_NOT_FOUND");

                var existingPhrase = await _context.LessonPhrases
                    .FirstOrDefaultAsync(p => p.Id == dto.PhraseId)
                    ?? throw new NotFoundException($"Phrase with ID {dto.PhraseId} not found", "PHRASE_NOT_FOUND");

                if (dto.PhraseText != null)
                {
                    existingPhrase.PhraseText = dto.PhraseText;
                }

                if (dto.Translation != null)
                {
                    existingPhrase.Translation = dto.Translation;
                }

                if (dto.ImageUrl != null)
                {
                    existingPhrase.ImageUrl = dto.ImageUrl;
                }

                _context.LessonPhrases.Update(existingPhrase);
                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                _logger.LogInformation("Successfully updated phrase with ID: {PhraseId}", dto.PhraseId);

                return existingPhrase;
            }
            catch (NotFoundException ex)
            {
                await transaction.RollbackAsync();
                _logger.LogWarning(ex, "Phrase not found for update: {PhraseId}", dto.PhraseId);
                throw;
            }
            catch (DbUpdateException ex)
            {
                await transaction.RollbackAsync();
                _logger.LogError(ex, "Database error while updating phrase with ID: {PhraseId}", dto.PhraseId);
                throw new RepositoryException($"Database error occurred while updating phrase with ID {dto.PhraseId}", "DB_UPDATE_ERROR", ex);
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                _logger.LogError(ex, "Unexpected error updating phrase with ID: {PhraseId}", dto.PhraseId);
                throw new RepositoryException($"An unexpected error occurred while updating phrase with ID {dto.PhraseId}", "UNEXPECTED_ERROR", ex);
            }
        }
    }
}