using Application.Services.Interfaces.IRepository.Lesons;
using Domain.Models;
using Infrastructure.Data;
using LangLearningAPI.Exceptions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Persistance.Repository.Lesons.QuizLeson
{
    public class LessonQuizRepository : IQuizRepository
    {
        private readonly LanguageLearningDbContext _context;
        private readonly ILogger<LessonQuizRepository> _logger;

        public LessonQuizRepository(LanguageLearningDbContext context, ILogger<LessonQuizRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<Quiz> CreateQuizAsync(Quiz entity)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                _logger.LogInformation("Creating new quiz for lesson {LessonId}", entity.LessonId);

                var lessonExists = await _context.Lessons.AnyAsync(l => l.Id == entity.LessonId);
                if (!lessonExists)
                    throw new NotFoundException($"Lesson with ID {entity.LessonId} not found", "LESSON_NOT_FOUND");

                await _context.Quizzes.AddAsync(entity);
                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                _logger.LogInformation("Successfully created quiz with ID: {QuizId}", entity.Id);
                
                return entity;
            }
            catch (DbUpdateException ex) when (ex.InnerException?.Message.Contains("unique constraint") ?? false)
            {
                await transaction.RollbackAsync();
                _logger.LogWarning(ex, "Duplicate quiz detected in lesson {LessonId}", entity.LessonId);
                throw new RepositoryException("Quiz of this type already exists in this lesson", "DUPLICATE_QUIZ", ex);
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                _logger.LogError(ex, "Error creating quiz");
                throw new RepositoryException("Error creating quiz", "CREATE_ERROR", ex);
            }
        }

        public async Task<Quiz> DeleteQuizAsync(int id)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                _logger.LogInformation("Deleting quiz with ID: {QuizId}", id);

                var quiz = await _context.Quizzes
                    .Include(q => q.Questions)
                    .FirstOrDefaultAsync(q => q.Id == id)
                    ?? throw new NotFoundException($"Quiz with ID {id} not found", "QUIZ_NOT_FOUND");

                _context.Quizzes.Remove(quiz);
                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                _logger.LogInformation("Successfully deleted quiz with ID: {QuizId}", id);
                
                return quiz;
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                _logger.LogError(ex, "Error deleting quiz with ID: {QuizId}", id);
                throw new RepositoryException("Error deleting quiz", "DELETE_ERROR", ex);
            }
        }

        public async Task<IEnumerable<Quiz>> GetAllQuizzesAsync()
        {
            try
            {
                return await _context.Quizzes
                    .AsNoTracking()
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting all quizzes");
                throw new RepositoryException("Error getting quizzes", "QUERY_ERROR", ex);
            }
        }

        public async Task<Quiz> GetQuizByIdAsync(int id)
        {
            try
            {
                return await _context.Quizzes
                    .Include(q => q.Questions)
                    .FirstOrDefaultAsync(q => q.Id == id)
                    ?? throw new NotFoundException($"Quiz with ID {id} not found", "QUIZ_NOT_FOUND");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting quiz by ID: {QuizId}", id);
                throw new RepositoryException("Error getting quiz", "QUERY_ERROR", ex);
            }
        }

        public async Task<IEnumerable<Quiz>> GetQuizzesByLessonIdAsync(int lessonId)
        {
            try
            {
                var lessonExists = await _context.Lessons.AnyAsync(l => l.Id == lessonId);
                if (!lessonExists)
                    throw new NotFoundException($"Lesson with ID {lessonId} not found", "LESSON_NOT_FOUND");

                return await _context.Quizzes
                    .Where(q => q.LessonId == lessonId)
                    .AsNoTracking()
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting quizzes for lesson {LessonId}", lessonId);
                throw new RepositoryException("Error getting quizzes for lesson", "QUERY_ERROR", ex);
            }
        }

        public async Task<bool> QuizExistsInLessonAsync(int lessonId, string type)
        {
            try
            {
                return await _context.Quizzes
                    .AnyAsync(q => q.LessonId == lessonId && q.Type == type);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error checking quiz existence in lesson");
                throw new RepositoryException("Error checking quiz existence", "QUERY_ERROR", ex);
            }
        }

        public async Task<bool> QuizExistsAsync(int id)
        {
            try
            {
                return await _context.Quizzes.AnyAsync(q => q.Id == id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error checking if quiz with ID {QuizId} exists", id);
                throw new RepositoryException("Failed to check quiz existence", "QUERY_FAILED", ex);
            }
        }

        public async Task<Quiz> UpdateQuizAsync(Quiz entity)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                _logger.LogInformation("Updating quiz with ID: {QuizId}", entity.Id);

                var existingQuiz = await _context.Quizzes
                    .FirstOrDefaultAsync(q => q.Id == entity.Id)
                    ?? throw new NotFoundException($"Quiz with ID {entity.Id} not found", "QUIZ_NOT_FOUND");

                _context.Entry(existingQuiz).CurrentValues.SetValues(entity);

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                _logger.LogInformation("Successfully updated quiz with ID: {QuizId}", entity.Id);
                
                return existingQuiz;
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                _logger.LogError(ex, "Error updating quiz with ID: {QuizId}", entity.Id);
                throw new RepositoryException("Error updating quiz", "UPDATE_ERROR", ex);
            }
        }
    }
}
