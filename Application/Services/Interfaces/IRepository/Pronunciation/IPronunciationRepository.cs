using Domain.Models;

namespace Application.Services.Interfaces.IRepository.Pronunciation
{
    public interface IPronunciationRepository
    {
        Task<IEnumerable<PronunciationCategory>> GetAllCategoriesAsync();
        
        Task<PronunciationCategory?> GetCategoryByIdAsync(int id);
        
        Task<PronunciationCategory?> CreateCategoryAsync(PronunciationCategory category);
        
        Task<bool> UpdateCategoryAsync(int id, PronunciationCategory updatedCategory);
        
        Task<bool> DeleteCategoryAsync(int id);


        Task<IEnumerable<WordItem>> GetWordsByCategoryIdAsync(int categoryId);
        
        Task<WordItem?> AddWordToCategoryAsync(int categoryId, WordItem word);
        
        Task<WordItem?> GetWordByIdAsync(int id);
        
        Task<bool> UpdateWordAsync(int wordId, WordItem wordPatch);
        
        Task<bool> DeleteWordAsync(int wordId);
    }

}
