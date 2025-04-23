using Application.DtoModels.MainQuestions;

namespace Application.Services.Interfaces.IServices.MainQuestions
{
    public interface IMainQuestionService
    {
        Task<IEnumerable<MainQuestionDto>> GetAllCategoriesAsync();
        
        Task<MainQuestionDto?> GetCategoryByIdAsync(int id);
        
        Task<MainQuestionDto?> CreateCategoryAsync(CreateMainQuestionDto dto);
        
        Task<MainQuestionDto?> UpdateCategoryAsync(int id, UpdateMainQuestionDto dto);
        
        Task<bool> DeleteCategoryAsync(int id);

        Task<IEnumerable<MainQuestionWordDto>> GetWordsByCategoryIdAsync(int categoryId);
        
        Task<MainQuestionWordDto?> AddWordToMainQuestionAsync(CreateMainQuestionWordDto dto);
        
        Task<bool> UpdateWordAsync(int wordId, UpdateMainQuestionWordDto dto);
        
        Task<bool> DeleteWordAsync(int wordId);
    }
}
