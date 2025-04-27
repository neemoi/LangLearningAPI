using Application.Services.Interfaces.IRepository.KidQuiz;
using Domain.Models;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Persistance.Repository.KidQuiz
{
    public class KidQuizAnswerRepository : IKidQuizAnswerRepository
    {
        private readonly LanguageLearningDbContext _context;
        private readonly ILogger<KidQuizAnswerRepository> _logger;

        public KidQuizAnswerRepository(LanguageLearningDbContext context, ILogger<KidQuizAnswerRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<KidQuizAnswer?> GetByIdKidQuizAnswerAsync(int id)
        {
            try
            {
                return await _context.KidQuizAnswers.FindAsync(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error retrieving KidQuizAnswer with Id {id}");
                throw;
            }
        }

        public async Task<IEnumerable<KidQuizAnswer>> GetByQuestionIdAsync(int questionId)
        {
            try
            {
                return await _context.KidQuizAnswers
                    .Where(a => a.QuestionId == questionId)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error retrieving KidQuizAnswers for QuestionId {questionId}");
                throw;
            }
        }

        public async Task<KidQuizAnswer> AddKidQuizAnswerAsync(KidQuizAnswer answer)
        {
            try
            {
                await _context.KidQuizAnswers.AddAsync(answer);
                await _context.SaveChangesAsync();
                
                return answer;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding KidQuizAnswer");
                throw;
            }
        }

        public async Task<KidQuizAnswer> UpdateKidQuizAnswerAsync(KidQuizAnswer answer)
        {
            try
            {
                var existingAnswer = await _context.KidQuizAnswers.FindAsync(answer.Id);
                if (existingAnswer == null)
                {
                    _logger.LogWarning($"KidQuizAnswer with Id {answer.Id} not found for update.");
                    throw new Exception($"KidQuizAnswer with Id {answer.Id} not found.");
                }

                if (!string.IsNullOrWhiteSpace(answer.AnswerText))
                    existingAnswer.AnswerText = answer.AnswerText;

                if (existingAnswer.IsCorrect != answer.IsCorrect)
                    existingAnswer.IsCorrect = answer.IsCorrect;

                if (answer.QuestionId != 0 && answer.QuestionId != existingAnswer.QuestionId)
                    existingAnswer.QuestionId = answer.QuestionId;

                _context.KidQuizAnswers.Update(existingAnswer);
                await _context.SaveChangesAsync();
                
                return existingAnswer;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error updating KidQuizAnswer with Id {answer.Id}");
                throw;
            }
        }

        public async Task<KidQuizAnswer> DeleteKidQuizAnswerAsync(int id)
        {
            try
            {
                var answer = await _context.KidQuizAnswers.FindAsync(id);
                if (answer == null)
                {
                    _logger.LogWarning($"KidQuizAnswer with Id {id} not found for deletion.");
                    throw new Exception($"KidQuizAnswer with Id {id} not found.");
                }

                _context.KidQuizAnswers.Remove(answer);
                await _context.SaveChangesAsync();
                
                return answer;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error deleting KidQuizAnswer with Id {id}");
                throw;
            }
        }
    }
}
