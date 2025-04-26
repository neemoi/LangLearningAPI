using Application.Services.Interfaces.IRepository.KidQuiz;
using Domain.Models;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Persistance.Repository.KidQuiz
{
    public class KidWordCardRepository : IKidWordCardRepository
    {
        private readonly LanguageLearningDbContext _context;
        private readonly ILogger<KidWordCardRepository> _logger;

        public KidWordCardRepository(LanguageLearningDbContext context, ILogger<KidWordCardRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<KidWordCard?> GetByIdAsync(int id)
        {
            try
            {
                return await _context.KidWordCards.FindAsync(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to retrieve KidWordCard with ID {Id}", id);
                return null;
            }
        }

        public async Task<IEnumerable<KidWordCard>> GetAllAsync()
        {
            try
            {
                return await _context.KidWordCards.ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to retrieve all KidWordCards");
                return Enumerable.Empty<KidWordCard>();
            }
        }

        public async Task<KidWordCard> AddAsync(KidWordCard wordCard)
        {
            try
            {
                await _context.KidWordCards.AddAsync(wordCard);
                await _context.SaveChangesAsync();
               
                return wordCard;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to add new KidWordCard");
                throw;
            }
        }

        public async Task<KidWordCard?> UpdateAsync(KidWordCard wordCard)
        {
            try
            {
                var existingCard = await _context.KidWordCards.FindAsync(wordCard.Id);
                if (existingCard == null)
                {
                    return null;
                }

                existingCard.Word = !string.IsNullOrEmpty(wordCard.Word) ? wordCard.Word : existingCard.Word;
                existingCard.ImageUrl = !string.IsNullOrEmpty(wordCard.ImageUrl) ? wordCard.ImageUrl : existingCard.ImageUrl;
                existingCard.AudioUrl = !string.IsNullOrEmpty(wordCard.AudioUrl) ? wordCard.AudioUrl : existingCard.AudioUrl;
                existingCard.LessonId = wordCard.LessonId != 0 ? wordCard.LessonId : existingCard.LessonId;

                await _context.SaveChangesAsync();
                
                return existingCard;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to update KidWordCard with ID {Id}", wordCard.Id);
                return null;
            }
        }

        public async Task<bool> DeleteAsync(int id)
        {
            try
            {
                var card = await _context.KidWordCards.FindAsync(id);
                if (card == null)
                {
                    return false;
                }

                _context.KidWordCards.Remove(card);
                await _context.SaveChangesAsync();
                
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to delete KidWordCard with ID {Id}", id);
                return false;
            }
        }

        public async Task<IEnumerable<KidWordCard>> GetWordCardsByLessonIdAsync(int lessonId)
        {
            try
            {
                return await _context.KidWordCards
                    .Where(c => c.LessonId == lessonId)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to retrieve KidWordCards for LessonId {LessonId}", lessonId);
                return Enumerable.Empty<KidWordCard>();
            }
        }
    }
}