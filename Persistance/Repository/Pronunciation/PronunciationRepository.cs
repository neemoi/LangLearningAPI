using Application.Services.Interfaces.IRepository.Pronunciation;
using Domain.Models;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Persistance.Repository.Pronunciation
{
    public class PronunciationRepository : IPronunciationRepository
    {
        private readonly LanguageLearningDbContext _context;
        private readonly ILogger<PronunciationRepository> _logger;

        public PronunciationRepository(LanguageLearningDbContext context, ILogger<PronunciationRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<WordItem?> GetWordByIdAsync(int id)
        {
            if (id <= 0)
            {
                _logger.LogWarning("Invalid word ID: {Id}", id);
                return null;
            }

            return await _context.WordItems.FindAsync(id);
        }

        public async Task<IEnumerable<PronunciationCategory>> GetAllCategoriesAsync()
        {
            return await _context.PronunciationCategories
                .Include(c => c.WordItems)
                .ToListAsync();
        }

        public async Task<PronunciationCategory?> GetCategoryByIdAsync(int id)
        {
            if (id <= 0)
            {
                _logger.LogWarning("Invalid category ID: {Id}", id);
                return null;
            }

            return await _context.PronunciationCategories
                .Include(c => c.WordItems)
                .FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<PronunciationCategory?> CreateCategoryAsync(PronunciationCategory category)
        {
            if (category == null)
            {
                _logger.LogWarning("Cannot create category: category object is null");
                return null;
            }

            if (string.IsNullOrWhiteSpace(category.Name))
            {
                _logger.LogWarning("Cannot create category: name is required");
                return null;
            }

            try
            {
                _context.PronunciationCategories.Add(category);
                await _context.SaveChangesAsync();
                
                return category;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception occurred while creating category");
                return null;
            }
        }

        public async Task<bool> UpdateCategoryAsync(int id, PronunciationCategory updatedCategory)
        {
            if (id <= 0)
            {
                _logger.LogWarning("Invalid category ID for update: {Id}", id);
                return false;
            }

            if (updatedCategory == null)
            {
                _logger.LogWarning("Cannot update category: updatedCategory object is null");
                return false;
            }

            var existing = await _context.PronunciationCategories.FindAsync(id);
            if (existing == null)
            {
                _logger.LogWarning("Category with ID {Id} not found", id);
                
                return false;
            }

            try
            {
                if (!string.IsNullOrWhiteSpace(updatedCategory.Name))
                    existing.Name = updatedCategory.Name;

                await _context.SaveChangesAsync();
                
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception occurred while updating category with ID {Id}", id);
                return false;
            }
        }

        public async Task<bool> DeleteCategoryAsync(int id)
        {
            if (id <= 0)
            {
                _logger.LogWarning("Invalid category ID for delete: {Id}", id);
                return false;
            }

            var entity = await _context.PronunciationCategories.FindAsync(id);
            if (entity == null)
            {
                _logger.LogWarning("Category with ID {Id} not found for deletion", id);
                return false;
            }

            try
            {
                _context.PronunciationCategories.Remove(entity);
                await _context.SaveChangesAsync();
                
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception occurred while deleting category with ID {Id}", id);
                return false;
            }
        }

        public async Task<IEnumerable<WordItem>> GetWordsByCategoryIdAsync(int categoryId)
        {
            if (categoryId <= 0)
            {
                _logger.LogWarning("Invalid category ID to fetch words: {CategoryId}", categoryId);
                return Enumerable.Empty<WordItem>();
            }

            return await _context.WordItems
                .Where(w => w.CategoryId == categoryId)
                .ToListAsync();
        }

        public async Task<WordItem?> AddWordToCategoryAsync(int categoryId, WordItem word)
        {
            if (categoryId <= 0)
            {
                _logger.LogWarning("Invalid category ID: {CategoryId}", categoryId);
                return null;
            }

            if (word == null)
            {
                _logger.LogWarning("Cannot add word: word object is null");
                return null;
            }

            if (string.IsNullOrWhiteSpace(word.Name))
            {
                _logger.LogWarning("Cannot add word: name is required");
                return null;
            }

            var categoryExists = await _context.PronunciationCategories.AnyAsync(c => c.Id == categoryId);
            if (!categoryExists)
            {
                _logger.LogWarning("Category with ID {CategoryId} not found", categoryId);
                return null;
            }

            try
            {
                word.CategoryId = categoryId;
                _context.WordItems.Add(word);
                await _context.SaveChangesAsync();
                
                return word;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception occurred while adding word to category {CategoryId}", categoryId);
                return null;
            }
        }

        public async Task<bool> UpdateWordAsync(int wordId, WordItem wordPatch)
        {
            if (wordId <= 0)
            {
                _logger.LogWarning("Invalid word ID for update: {WordId}", wordId);
                return false;
            }

            if (wordPatch == null)
            {
                _logger.LogWarning("Cannot update word: wordPatch object is null");
                return false;
            }

            var existing = await _context.WordItems.FindAsync(wordId);
            if (existing == null)
            {
                _logger.LogWarning("Word with ID {WordId} not found", wordId);
                return false;
            }

            try
            {
                if (wordPatch.Name != null)
                    existing.Name = wordPatch.Name;

                if (wordPatch.ImagePath != null)
                    existing.ImagePath = wordPatch.ImagePath;

                await _context.SaveChangesAsync();
               
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception occurred while updating word with ID {WordId}", wordId);
                return false;
            }
        }

        public async Task<bool> DeleteWordAsync(int wordId)
        {
            if (wordId <= 0)
            {
                _logger.LogWarning("Invalid word ID for deletion: {WordId}", wordId);
                return false;
            }

            var word = await _context.WordItems.FindAsync(wordId);
            if (word == null)
            {
                _logger.LogWarning("Word with ID {WordId} not found for deletion", wordId);
                return false;
            }

            try
            {
                _context.WordItems.Remove(word);
                await _context.SaveChangesAsync();
                
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception occurred while deleting word with ID {WordId}", wordId);
                return false;
            }
        }
    }
}