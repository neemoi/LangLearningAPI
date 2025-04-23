using Application.Services.Interfaces.IRepository.MainQuestions;
using Domain.Models;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Persistence.Repository.MainQuestions
{
    public class MainQuestionRepository : IMainQuestionRepository
    {
        private readonly LanguageLearningDbContext _context;
        private readonly ILogger<MainQuestionRepository> _logger;

        public MainQuestionRepository(LanguageLearningDbContext context, ILogger<MainQuestionRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<IEnumerable<MainQuestion>> GetAllCategoriesAsync()
        {
            try
            {
                return await _context.MainQuestions
                    .Include(q => q.Words)
                    .AsNoTracking()
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting all categories");
                throw;
            }
        }

        public async Task<MainQuestion?> GetCategoryByIdAsync(int id)
        {
            try
            {
                return await _context.MainQuestions
                    .Include(q => q.Words)
                    .AsNoTracking()
                    .FirstOrDefaultAsync(q => q.Id == id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting category by ID: {Id}", id);
                throw;
            }
        }

        public async Task<MainQuestion?> CreateCategoryAsync(MainQuestion category)
        {
            try
            {
                _context.MainQuestions.Add(category);
                await _context.SaveChangesAsync();
                
                return category;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating category");
                throw;
            }
        }

        public async Task<MainQuestion?> UpdateCategoryAsync(int id, MainQuestion updatedCategory)
        {
            try
            {
                var existing = await _context.MainQuestions.FindAsync(id);
                if (existing == null)
                {
                    _logger.LogWarning("Category with ID {Id} not found", id);
                    return null;
                }

                if (!string.IsNullOrWhiteSpace(updatedCategory.Name))
                    existing.Name = updatedCategory.Name;

                if (!string.IsNullOrWhiteSpace(updatedCategory.ImagePath))
                    existing.ImagePath = updatedCategory.ImagePath;

                await _context.SaveChangesAsync();
                
                return existing;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating category ID: {Id}", id);
                throw;
            }
        }

        public async Task<bool> DeleteCategoryAsync(int id)
        {
            try
            {
                var category = await _context.MainQuestions.FindAsync(id);
                if (category == null)
                {
                    _logger.LogWarning("Category with ID {Id} not found", id);
                    return false;
                }

                _context.MainQuestions.Remove(category);
                await _context.SaveChangesAsync();
                
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting category ID: {Id}", id);
                throw;
            }
        }

        public async Task<IEnumerable<MainQuestionWord>> GetWordsByCategoryIdAsync(int categoryId)
        {
            try
            {
                return await _context.MainQuestionWords
                    .Where(w => w.MainQuestionId == categoryId)
                    .AsNoTracking()
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting words for category ID: {CategoryId}", categoryId);
                throw;
            }
        }

        public async Task<MainQuestionWord?> AddWordToMainQuestionAsync(int mainQuestionId, MainQuestionWord word)
        {
            try
            {
                word.MainQuestionId = mainQuestionId;
                _context.MainQuestionWords.Add(word);
                await _context.SaveChangesAsync();
                
                return word;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding word to question ID: {MainQuestionId}", mainQuestionId);
                throw;
            }
        }

        public async Task<bool> UpdateWordAsync(int wordId, MainQuestionWord wordPatch)
        {
            try
            {
                var existing = await _context.MainQuestionWords.FindAsync(wordId);
                if (existing == null)
                {
                    _logger.LogWarning("Word with ID {WordId} not found", wordId);
                    return false;
                }

                if (!string.IsNullOrWhiteSpace(wordPatch.Name))
                    existing.Name = wordPatch.Name;

                if (!string.IsNullOrWhiteSpace(wordPatch.ImagePath))
                    existing.ImagePath = wordPatch.ImagePath;

                await _context.SaveChangesAsync();
                
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating word ID: {WordId}", wordId);
                throw;
            }
        }

        public async Task<bool> DeleteWordAsync(int wordId)
        {
            try
            {
                var word = await _context.MainQuestionWords.FindAsync(wordId);
                if (word == null)
                {
                    _logger.LogWarning("Word with ID {WordId} not found", wordId);
                    return false;
                }

                _context.MainQuestionWords.Remove(word);
                await _context.SaveChangesAsync();
                
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting word ID: {WordId}", wordId);
                throw;
            }
        }
    }
}
