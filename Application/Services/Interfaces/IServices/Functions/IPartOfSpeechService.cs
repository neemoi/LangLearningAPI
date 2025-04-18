using Application.DtoModels.Functions;

namespace Application.Services.Interfaces.IServices.Functions
{
    public interface IPartOfSpeechService
    {
        Task<PartOfSpeechDto?> GetPartOfSpeechByIdAsync(int id);
        
        Task<IEnumerable<PartOfSpeechDto>> GetAllPartOfSpeechAsync();
        
        Task<PartOfSpeechDto> AddPartOfSpeechAsync(CreatePartOfSpeechDto dto);
        
        Task<PartOfSpeechDto> UpdatePartOfSpeechAsync(FunctionWordUpdateDto dto);
        
        Task<bool> DeletePartOfSpeechAsync(int id);
    }
}
