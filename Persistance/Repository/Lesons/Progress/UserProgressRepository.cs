using Application.DtoModels.Lessons.Progress;
using Application.Services.Interfaces.IServices.Lesons;
using Domain.Models;
using Infrastructure.Data;
using LangLearningAPI.Exceptions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Persistance.Repository.Lesons.Progress
{
    public class UserProgressRepository : IUserProgressRepository
    {
        private readonly LanguageLearningDbContext _context;
        private readonly ILogger<UserProgressRepository> _logger;

        public UserProgressRepository(LanguageLearningDbContext context, ILogger<UserProgressRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<UserProgress> AddUserProgressAsync(UserProgress userProgress)
        {
            try
            {
                ValidateUserProgress(userProgress);

                var exists = await _context.UserProgress
                    .AnyAsync(up => up.UserId == userProgress.UserId &&
                                    up.LessonId == userProgress.LessonId);

                if (exists)
                {
                    _logger.LogWarning("Progress already exists for user {UserId}, lesson {LessonId}",
                        userProgress.UserId, userProgress.LessonId);
                    throw new InvalidOperationException("Progress already exists for this lesson and user.");
                }

                await _context.UserProgress.AddAsync(userProgress);
                await _context.SaveChangesAsync();
                
                return userProgress;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding user progress for user {UserId}", userProgress.UserId);
                throw;
            }
        }

        public async Task<UserProgress> UpdateUserProgressAsync(UserProgress userProgress)
        {
            try
            {
                ValidateUserProgress(userProgress);

                var existing = await _context.UserProgress
                    .FirstOrDefaultAsync(up => up.UserId == userProgress.UserId &&
                                               up.LessonId == userProgress.LessonId);

                if (existing == null)
                {
                    _logger.LogWarning("Progress not found for user {UserId}, lesson {LessonId}",
                        userProgress.UserId, userProgress.LessonId);
                    throw new NotFoundException("Progress not found for update.");
                }

                existing.Score = userProgress.Score;
                existing.CompletedAt = userProgress.CompletedAt;
                existing.LearnedWords = userProgress.LearnedWords;
                existing.QuizId = userProgress.QuizId;

                _context.UserProgress.Update(existing);
                await _context.SaveChangesAsync();
                
                return existing;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating progress for user {UserId}", userProgress.UserId);
                throw;
            }
        }

        public async Task<UserLessonProgressViewDto> GetFullProgressAsync(string userId, int lessonId)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(userId))
                    throw new ArgumentException("UserId is required");

                if (lessonId <= 0)
                    throw new ArgumentException("LessonId must be positive");

                var userProgress = await _context.UserProgress
                    .Include(up => up.Lesson)
                    .Include(up => up.Quiz)
                        .ThenInclude(q => q.Questions)
                    .FirstOrDefaultAsync(up => up.UserId == userId && up.LessonId == lessonId);

                if (userProgress == null)
                {
                    throw new NotFoundException("User progress not found.");
                }

                var wordProgress = await _context.UserWordProgress
                    .Where(wp => wp.UserId == userId && wp.LessonId == lessonId)
                    .Include(wp => wp.Word)
                    .ToListAsync();

                return new UserLessonProgressViewDto
                {
                    UserId = userId,
                    LessonTitle = userProgress.Lesson?.Title,
                    LessonDescription = userProgress.Lesson?.Description,
                    VideoUrl = userProgress.Lesson?.VideoUrl,
                    PdfUrl = userProgress.Lesson?.PdfUrl,
                    QuizType = userProgress.Quiz?.Type,
                    TotalQuestions = userProgress.Quiz?.Questions?.Count ?? 0,
                    CorrectAnswers = wordProgress.Count(w => w.IsCorrect),
                    LearnedWords = userProgress.LearnedWords,
                    Score = userProgress.Score,
                    CompletedAt = userProgress.CompletedAt,
                    Words = wordProgress.Select(wp => new WordProgressDto
                    {
                        WordId = wp.WordId,
                        WordText = wp.Word?.Name ?? "[unknown]",
                        IsCorrect = wp.IsCorrect,
                        QuestionType = wp.QuestionType ?? "unknown"
                    }).ToList()
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting full progress for user {UserId}, lesson {LessonId}",
                    userId, lessonId);
                throw;
            }
        }

        public async Task<UserWordStatsDto> GetWordStatsAsync(string userId, int lessonId)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(userId))
                    throw new ArgumentException("UserId is required");

                if (lessonId <= 0)
                    throw new ArgumentException("LessonId must be positive");

                var totalWordsInLesson = await _context.LessonWords
                    .CountAsync(w => w.LessonId == lessonId);

                var learnedWordsInLesson = await _context.UserWordProgress
                    .Where(wp => wp.UserId == userId &&
                                 wp.LessonId == lessonId &&
                                 wp.IsCorrect)
                    .Select(wp => wp.WordId)
                    .Distinct()
                    .CountAsync();

                var totalWordsOverall = await _context.LessonWords.CountAsync();

                var learnedWordsOverall = await _context.UserWordProgress
                    .Where(wp => wp.UserId == userId && wp.IsCorrect)
                    .Select(wp => wp.WordId)
                    .Distinct()
                    .CountAsync();

                return new UserWordStatsDto
                {
                    TotalWordsInLesson = totalWordsInLesson,
                    LearnedWordsInLesson = learnedWordsInLesson,
                    TotalWordsOverall = totalWordsOverall,
                    LearnedWordsOverall = learnedWordsOverall
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting word stats for user {UserId}, lesson {LessonId}",
                    userId, lessonId);
                throw;
            }
        }

        public async Task<Domain.Models.UserWordProgress> AddWordProgressAsync(Domain.Models.UserWordProgress progress)
        {
            try
            {
                ValidateWordProgress(progress);

                await _context.UserWordProgress.AddAsync(progress);
                await _context.SaveChangesAsync();
                
                return progress;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to add word progress for user {UserId}, word {WordId}",
                    progress.UserId, progress.WordId);
                throw;
            }
        }

        public async Task<Domain.Models.UserWordProgress> UpdateWordProgressAsync(Domain.Models.UserWordProgress progress)
        {
            try
            {
                ValidateWordProgress(progress, checkId: true);

                var existing = await _context.UserWordProgress.FindAsync(progress.Id);
                if (existing == null)
                {
                    throw new NotFoundException("Word progress not found");
                }

                existing.QuestionType = progress.QuestionType;
                existing.IsCorrect = progress.IsCorrect;

                await _context.SaveChangesAsync();
                
                return existing;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating word progress {Id}", progress.Id);
                throw;
            }
        }

        public async Task<Domain.Models.UserWordProgress> GetWordProgressAsync(int id)
        {
            try
            {
                return await _context.UserWordProgress
                    .Include(x => x.Word)
                    .FirstOrDefaultAsync(x => x.Id == id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting word progress {Id}", id);
                throw;
            }
        }

        public async Task<List<Domain.Models.UserWordProgress>> GetWordProgressesByUserAndLessonAsync(string userId, int lessonId)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(userId))
                    throw new ArgumentException("UserId is required");

                if (lessonId <= 0)
                    throw new ArgumentException("LessonId must be positive");

                return await _context.UserWordProgress
                    .Include(x => x.Word)
                    .Where(x => x.UserId == userId && x.LessonId == lessonId)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting word progresses for user {UserId}, lesson {LessonId}",
                    userId, lessonId);
                throw;
            }
        }

        private void ValidateUserProgress(UserProgress progress)
        {
            if (progress == null)
                throw new ArgumentNullException(nameof(progress));

            if (string.IsNullOrWhiteSpace(progress.UserId))
                throw new ArgumentException("UserId is required");

            if (progress.LessonId <= 0)
                throw new ArgumentException("LessonId must be positive");

            if (progress.Score < 0)
                throw new ArgumentException("Score cannot be negative");

            if (progress.LearnedWords < 0)
                throw new ArgumentException("LearnedWords cannot be negative");
        }

        private void ValidateWordProgress(Domain.Models.UserWordProgress progress, bool checkId = false)
        {
            if (progress == null)
                throw new ArgumentNullException(nameof(progress));

            if (checkId && progress.Id <= 0)
                throw new ArgumentException("Id must be positive");

            if (string.IsNullOrWhiteSpace(progress.UserId))
                throw new ArgumentException("UserId is required");

            if (progress.LessonId <= 0)
                throw new ArgumentException("LessonId must be positive");

            if (progress.WordId <= 0)
                throw new ArgumentException("WordId must be positive");

            if (!Enum.IsDefined(typeof(QuestionType), progress.QuestionType))
                throw new ArgumentException($"Invalid QuestionType: {progress.QuestionType}");
        }
    }
}
