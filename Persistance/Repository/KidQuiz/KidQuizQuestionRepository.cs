using Application.DtoModels.KidQuiz;
using Application.Services.Interfaces.IRepository.KidQuiz;
using AutoMapper;
using Domain.Models;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Persistance.Repository.KidQuiz
{
    public class KidQuizQuestionRepository : IKidQuizQuestionRepository
    {
        private readonly LanguageLearningDbContext _context;
        private readonly ILogger<KidQuizQuestionRepository> _logger;
        private readonly IMapper _mapper;

        public KidQuizQuestionRepository(LanguageLearningDbContext context, ILogger<KidQuizQuestionRepository> logger, IMapper mapper)
        {
            _context = context;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task<KidQuizQuestion?> GetByIdQuizQuestionAsync(int id)
        {
            try
            {
                var question = await _context.KidQuizQuestions
                    .Include(q => q.Answers)
                    .Include(q => q.Lesson)
                    .Include(q => q.QuizType)
                    .FirstOrDefaultAsync(q => q.Id == id);

                if (question == null)
                {
                    _logger.LogWarning($"KidQuizQuestion with ID {id} not found.");
                    return null;
                }

                _logger.LogInformation($"KidQuizQuestion with ID {id} retrieved successfully.");
                
                return question;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error retrieving KidQuizQuestion with ID {id}: {ex.Message}");
                throw new Exception($"Error retrieving KidQuizQuestion with ID {id}: {ex.Message}", ex);
            }
        }

        public async Task<IEnumerable<KidQuizQuestionDto>> GetByLessonIdQuizQuestionAsync(int lessonId)
        {
            try
            {
                var questions = await _context.KidQuizQuestions
                    .Where(q => q.LessonId == lessonId)
                    .Include(q => q.Answers)
                    .Include(q => q.QuizType)
                    .Include(q => q.WordCard)
                    .ToListAsync();

                if (!questions.Any())
                {
                    _logger.LogWarning($"No KidQuizQuestions found for LessonId {lessonId}.");
                }

                _logger.LogInformation($"Retrieved {questions.Count} KidQuizQuestions for LessonId {lessonId}.");

                var questionDtos = _mapper.Map<IEnumerable<KidQuizQuestionDto>>(questions);
                
                return questionDtos;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error retrieving KidQuizQuestions for LessonId {lessonId}: {ex.Message}");
                throw new Exception($"Error retrieving KidQuizQuestions for LessonId {lessonId}: {ex.Message}", ex);
            }
        }

        public async Task<KidQuizQuestion?> AddQuizQuestionAsync(KidQuizQuestion question)
        {
            try
            {
                var lessonExists = await _context.KidLessons.AnyAsync(l => l.Id == question.LessonId);
                if (!lessonExists)
                    throw new ArgumentException($"Lesson with ID {question.LessonId} not found.");

                var quizTypeExists = await _context.KidQuizTypes.AnyAsync(q => q.Id == question.QuizTypeId);
                if (!quizTypeExists)
                    throw new ArgumentException($"QuizType with ID {question.QuizTypeId} not found.");

                var wordCardExists = await _context.KidWordCards.AnyAsync(w => w.Id == question.WordCardId);
                if (!wordCardExists)
                    throw new ArgumentException($"WordCard with ID {question.WordCardId} not found.");

                _context.KidQuizQuestions.Add(question);
                await _context.SaveChangesAsync();

                return question;
            }
            catch (ArgumentException ex)
            {
                _logger.LogWarning(ex.Message);
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding KidQuizQuestion");
                throw;
            }
        }

        public async Task<KidQuizQuestion?> UpdateQuizQuestionAsync(KidQuizQuestion question)
        {
            try
            {
                var existingQuestion = await _context.KidQuizQuestions
                    .Include(q => q.Answers)
                    .FirstOrDefaultAsync(q => q.Id == question.Id);

                if (existingQuestion == null) return null;

                _mapper.Map(question, existingQuestion);
                await _context.SaveChangesAsync();

                return existingQuestion;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating KidQuizQuestion with ID {QuestionId}", question.Id);
                throw;
            }
        }

        public async Task<KidQuizQuestion?> DeleteQuizQuestionAstnc(KidQuizQuestion question)
        {
            try
            {
                var existingQuestion = await _context.KidQuizQuestions
                    .FirstOrDefaultAsync(q => q.Id == question.Id);

                if (existingQuestion == null)
                {
                    _logger.LogWarning($"KidQuizQuestion with ID {question.Id} not found for deletion.");
                    return null;
                }

                _context.KidQuizQuestions.Remove(existingQuestion);
                await _context.SaveChangesAsync();

                _logger.LogInformation($"KidQuizQuestion with ID {question.Id} deleted successfully.");
                
                return existingQuestion;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error deleting KidQuizQuestion with ID {question.Id}: {ex.Message}");
                throw new Exception($"Error deleting KidQuizQuestion with ID {question.Id}: {ex.Message}", ex);
            }
        }
    }
}
