using Application.Services.Interfaces.IRepository.Lesons;
using AutoMapper;
using Domain.Models;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Persistance.Repository.Lesons.Leson
{
    public class LessonRepository : ILessonRepository
    {
        private readonly LanguageLearningDbContext _context;
        private readonly ILogger<LessonRepository> _logger;
        private readonly IMapper _mapper;

        public LessonRepository(LanguageLearningDbContext context, ILogger<LessonRepository> logger, IMapper mapper)
        {
            _context = context;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task<Lesson> GetLessonByIdAsync(int id)
        {

            try
            {
                if (id <= 0)
                {
                    _logger.LogWarning("Attempt to get lesson with invalid ID: {Id}", id);
                    throw new ArgumentException("ID must be a positive integer", nameof(id));
                }

                var lesson = await _context.Lessons
                  .Include(l => l.Words)
                  .Include(l => l.Phrases)
                  .Include(l => l.Quizzes)
                  .FirstOrDefaultAsync(l => l.Id == id);

                if (lesson != null)
                {
                    foreach (var quiz in lesson.Quizzes)
                    {
                        await _context.Entry(quiz)
                            .Collection(q => q.Questions)
                            .Query()
                            .Include(q => q.Answers)
                            .LoadAsync();
                    }
                }

                if (lesson == null)
                {
                    _logger.LogWarning("Lesson with ID {Id} not found in database", id);
                    throw new KeyNotFoundException($"Lesson with ID {id} does not exist");
                }

                return lesson;
            }
            catch (Exception ex) when (ex is not KeyNotFoundException)
            {
                _logger.LogError(ex, "Error occurred while retrieving lesson with ID {Id}", id);
                throw new RepositoryException($"Database error occurred while retrieving lesson {id}", ex);
            }
        }

        public async Task<List<Lesson>> GetAllLessonsAsync()
        {
            try
            {
                var lessons = await _context.Lessons
                    .Include(l => l.Words)
                    .Include(l => l.Phrases)
                    .Include(l => l.Quizzes)
                    .ToListAsync();

                foreach (var lesson in lessons)
                {
                    foreach (var quiz in lesson.Quizzes)
                    {
                        await _context.Entry(quiz)
                            .Collection(q => q.Questions)
                            .Query()
                            .Include(q => q.Answers)
                            .LoadAsync();
                    }
                }

                if (!lessons.Any())
                {
                    _logger.LogWarning("No lessons found in database");
                    throw new KeyNotFoundException("No lessons exist in the database");
                }

                return lessons;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while retrieving all lessons");
                throw new RepositoryException("Database error occurred while retrieving lessons", ex);
            }
        }

        public async Task<Lesson> AddLessonAsync(Lesson lesson)
        {
            try
            {
                if (lesson == null)
                {
                    _logger.LogWarning("Attempt to add null lesson");
                    throw new ArgumentNullException(nameof(lesson), "Lesson cannot be null");
                }

                await _context.Lessons.AddAsync(lesson);
                await _context.SaveChangesAsync();

                _logger.LogInformation("Successfully added new lesson with ID {Id}", lesson.Id);
                return lesson;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while adding new lesson");
                throw new RepositoryException("Database error occurred while adding lesson", ex);
            }
        }

        public async Task<Lesson> UpdateLessonAsync(int id, Action<Lesson> updateAction)
        {
            await using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                if (id <= 0)
                {
                    _logger.LogWarning("Attempt to update lesson with invalid ID: {Id}", id);
                    throw new ArgumentException("ID must be a positive integer", nameof(id));
                }

                if (updateAction == null)
                {
                    _logger.LogWarning("Attempt to update lesson with null update action");
                    throw new ArgumentNullException(nameof(updateAction), "Update action cannot be null");
                }

                var lesson = await GetLessonByIdAsync(id);
                updateAction(lesson);

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                _logger.LogInformation("Successfully updated lesson with ID {Id}", id);
                return lesson;
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                _logger.LogError(ex, "Error occurred while updating lesson with ID {Id}", id);
                throw new RepositoryException($"Database error occurred while updating lesson {id}", ex);
            }
        }

        public async Task<Lesson?> DeleteLessonAsync(int id)
        {

            try
            {
                if (id <= 0)
                {
                    _logger.LogWarning("Attempt to delete lesson with invalid ID: {Id}", id);
                    throw new ArgumentException("ID must be a positive integer", nameof(id));
                }

                var lesson = await _context.Lessons
                    .Include(l => l.Words)
                    .Include(l => l.Phrases)
                    .Include(l => l.Quizzes)
                        .ThenInclude(q => q.Questions)
                            .ThenInclude(q => q.Answers)
                    .FirstOrDefaultAsync(l => l.Id == id);

                if (lesson == null)
                {
                    _logger.LogWarning("Lesson with ID {Id} not found for deletion", id);
                    return null;
                }

                var deletedLesson = _mapper.Map<Lesson>(lesson);

                _context.Lessons.Remove(lesson);
                await _context.SaveChangesAsync();

                return deletedLesson;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while deleting lesson with ID {Id}", id);
                throw new RepositoryException($"Database error occurred while deleting lesson {id}", ex);
            }
        }

        public async Task<List<LessonWord>> GetWordsByLessonIdAsync(int lessonId)
        {

            try
            {
                if (lessonId <= 0)
                {
                    _logger.LogWarning("Attempt to get words with invalid lesson ID: {LessonId}", lessonId);
                    throw new ArgumentException("Lesson ID must be a positive integer", nameof(lessonId));
                }

                await GetLessonByIdAsync(lessonId);

                var words = await _context.LessonWords
                    .Where(w => w.LessonId == lessonId)
                    .ToListAsync();

                if (words == null || !words.Any())
                {
                    _logger.LogWarning("No words found for lesson ID {LessonId}", lessonId);
                    throw new KeyNotFoundException($"No words exist for lesson {lessonId}");
                }

                return words;
            }
            catch (Exception ex) when (ex is not KeyNotFoundException)
            {
                _logger.LogError(ex, "Error occurred while retrieving words for lesson ID {LessonId}", lessonId);
                throw new RepositoryException($"Database error occurred while retrieving words for lesson {lessonId}", ex);
            }
        }

        public async Task<List<LessonPhrase>> GetPhrasesByLessonAsync(int lessonId)
        {

            try
            {
                if (lessonId <= 0)
                {
                    _logger.LogWarning("Attempt to get phrases with invalid lesson ID: {LessonId}", lessonId);
                    throw new ArgumentException("Lesson ID must be a positive integer", nameof(lessonId));
                }

                await GetLessonByIdAsync(lessonId);

                var phrases = await _context.LessonPhrases
                    .Where(p => p.LessonId == lessonId)
                    .ToListAsync();

                if (phrases == null || !phrases.Any())
                {
                    _logger.LogWarning("No phrases found for lesson ID {LessonId}", lessonId);
                    throw new KeyNotFoundException($"No phrases exist for lesson {lessonId}");
                }

                return phrases;
            }
            catch (Exception ex) when (ex is not KeyNotFoundException)
            {
                _logger.LogError(ex, "Error occurred while retrieving phrases for lesson ID {LessonId}", lessonId);
                throw new RepositoryException($"Database error occurred while retrieving phrases for lesson {lessonId}", ex);
            }
        }

        public async Task<List<Quiz>> GetQuizzesByLessonAsync(int lessonId)
        {

            try
            {
                if (lessonId <= 0)
                {
                    _logger.LogWarning("Attempt to get quizzes with invalid lesson ID: {LessonId}", lessonId);
                    throw new ArgumentException("Lesson ID must be a positive integer", nameof(lessonId));
                }

                await GetLessonByIdAsync(lessonId);

                var quizzes = await _context.Quizzes
                    .Where(q => q.LessonId == lessonId)
                    .Include(q => q.Questions)
                        .ThenInclude(q => q.Answers)
                    .ToListAsync();

                if (quizzes == null || !quizzes.Any())
                {
                    _logger.LogWarning("No quizzes found for lesson ID {LessonId}", lessonId);
                    throw new KeyNotFoundException($"No quizzes exist for lesson {lessonId}");
                }

                return quizzes;
            }
            catch (Exception ex) when (ex is not KeyNotFoundException)
            {
                _logger.LogError(ex, "Error occurred while retrieving quizzes for lesson ID {LessonId}", lessonId);
                throw new RepositoryException($"Database error occurred while retrieving quizzes for lesson {lessonId}", ex);
            }
        }
    }

    public class RepositoryException : Exception
    {
        public RepositoryException(string message) : base(message) { }
        public RepositoryException(string message, Exception inner) : base(message, inner) { }
    }
}