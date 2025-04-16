using Application.Services.Interfaces.IRepository.Nouns;
using Domain.Models;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Persistance.Repository.Nouns
{
    public class NounWordRepository : INounWordRepository
    {
        private readonly LanguageLearningDbContext _context;
        private readonly ILogger<NounWordRepository> _logger;

        public NounWordRepository(LanguageLearningDbContext context, ILogger<NounWordRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<IEnumerable<NounWord>> GetAllNounWordAsync()
        {
            try
            {
                return await _context.NounWords
                    .Include(w => w.AlphabetLetter)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while getting all noun words");
                throw;
            }
        }

        public async Task<NounWord?> GetNounWordByIdAsync(int id)
        {
            try
            {
                return await _context.NounWords
                    .Include(w => w.AlphabetLetter)
                    .FirstOrDefaultAsync(w => w.Id == id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error occurred while getting noun word with id {id}");
                throw;
            }
        }

        public async Task<IEnumerable<NounWord>> GetByLetterIdAsync(int letterId)
        {
            try
            {
                return await _context.NounWords
                    .Where(w => w.AlphabetLetterId == letterId)
                    .Include(w => w.AlphabetLetter)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error occurred while getting noun words for letter id {letterId}");
                throw;
            }
        }

        public async Task<NounWord> AddNounWordAsync(NounWord word)
        {
            try
            {
                _context.NounWords.Add(word);
                await _context.SaveChangesAsync();
                
                return word;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while adding noun word");
                throw;
            }
        }

        public async Task<NounWord> UpdateNounWordAsync(NounWord word)
        {
            try
            {
                var existingWord = await _context.NounWords.FindAsync(word.Id);
                if (existingWord == null)
                    throw new KeyNotFoundException($"Noun word with id {word.Id} not found");

                if (word.Name != null)
                    existingWord.Name = word.Name;

                if (word.ImageUrl != null)
                    existingWord.ImageUrl = word.ImageUrl;

                if (word.AlphabetLetterId != 0) 
                    existingWord.AlphabetLetterId = word.AlphabetLetterId;

                await _context.SaveChangesAsync();
                
                return existingWord;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error updating noun word with id {word.Id}");
                throw;
            }
        }

        public async Task<bool> DeleteNounWordAsync(int id)
        {
            try
            {
                var word = await _context.NounWords.FindAsync(id);
                if (word == null)
                    return false;

                _context.NounWords.Remove(word);
                await _context.SaveChangesAsync();
                
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error occurred while deleting noun word with id {id}");
                throw;
            }
        }
    }
}
