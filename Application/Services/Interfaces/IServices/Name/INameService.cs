using Application.DtoModels.Name.EnglishName;

namespace Application.Services.Interfaces.IServices.Name
{
    public interface INameService
    {
        Task<IEnumerable<EnglishNameDto>> GetAllEnglishNamesAsync();

        Task<EnglishNameDto?> GetEnglishNameByIdAsync(int id);
        
        Task<EnglishNameDto?> CreateEnglishNameAsync(CreateEnglishNameDto dto);
        
        Task<EnglishNameDto?> UpdateEnglishNameAsync(int id, UpdateEnglishNameDto dto);
       
        Task<bool> DeleteEnglishNameAsync(int id);
    }
}
