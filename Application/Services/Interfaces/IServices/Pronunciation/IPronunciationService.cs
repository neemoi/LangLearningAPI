using Application.DtoModels.Pronunciation;

namespace Application.Services.Interfaces.IServices.Pronunciation
{
    public interface IPronunciationService
    {
        Task<IEnumerable<PronunciationCategoryDto>> GetAllCategoriesAsync();
        
        Task<PronunciationCategoryDto?> GetCategoryByIdAsync(int id);
        
        Task<PronunciationCategoryDto?> CreateCategoryAsync(CreateCategoryDto dto);
        
        Task<PronunciationCategoryDto?> UpdateCategoryAsync(UpdateCategoryDto dto);
        
        Task<PronunciationCategoryDto?> DeleteCategoryAsync(int id);


        Task<IEnumerable<WordItemDto>> GetWordsByCategoryIdAsync(int categoryId);
        
        Task<WordItemDto?> AddWordToCategoryAsync(int categoryId, CreateWordItemDto dto);
        
        Task<WordItemDto> UpdateWordAsync(UpdateWordItemDto dto);
        
        Task<WordItemDto> DeleteWordAsync(int wordId);
    }
}
