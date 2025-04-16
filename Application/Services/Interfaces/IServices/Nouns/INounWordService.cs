using Application.DtoModels.Nouns;

namespace Application.Services.Interfaces.IServices.Nouns
{
    public interface INounWordService
    {
        Task<IEnumerable<NounWordDto>> GetAllWordsAsync();
        
        Task<NounWordDto?> GetWordByIdAsync(int id);
        
        Task<IEnumerable<NounWordDto>> GetWordsByLetterIdAsync(int letterId);
        
        Task<NounWordDto> AddWordAsync(NounWordDto wordDto);
        
        Task<NounWordDto> UpdateWordAsync(UpdateNounWordDto wordDto);
        
        Task<bool> DeleteWordAsync(int id);
    }
}