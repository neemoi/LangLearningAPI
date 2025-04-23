using Domain.Models;

namespace Application.Services.Interfaces.IRepository.MainQuestions
{
    public interface IMainQuestionRepository
    {
        Task<IEnumerable<MainQuestion>> GetAllCategoriesAsync();
        
        Task<MainQuestion?> GetCategoryByIdAsync(int id);
        
        Task<MainQuestion?> CreateCategoryAsync(MainQuestion category);
        
        Task<MainQuestion?> UpdateCategoryAsync(int id, MainQuestion updatedCategory);
        
        Task<bool> DeleteCategoryAsync(int id);

        Task<IEnumerable<MainQuestionWord>> GetWordsByCategoryIdAsync(int categoryId);
        
        Task<MainQuestionWord?> AddWordToMainQuestionAsync(int mainQuestionId, MainQuestionWord word);
        
        Task<bool> UpdateWordAsync(int wordId, MainQuestionWord wordPatch);
        
        Task<bool> DeleteWordAsync(int wordId);
    }
}
