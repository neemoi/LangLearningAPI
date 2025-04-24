using Application.DtoModels.Name.FemaleName;

namespace Application.Services.Interfaces.IServices.Name
{
    public interface IFemaleNameService
    {
        Task<IEnumerable<FemaleNameDto>> GetAllFemaleNamesAsync();
        
        Task<FemaleNameDto?> GetFemaleNameByIdAsync(int id);
        
        Task<FemaleNameDto?> CreateFemaleNameAsync(CreateFemaleNameDto dto);
        
        Task<FemaleNameDto?> UpdateFemaleNameAsync(int id, UpdateFemaleNameDto dto);
        
        Task<bool> DeleteFemaleNameAsync(int id);
    }
}
