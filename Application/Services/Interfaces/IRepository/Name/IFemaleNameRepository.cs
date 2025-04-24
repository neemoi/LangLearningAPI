using Domain.Models;

namespace Application.Services.Interfaces.IRepository.Name
{
    public interface IFemaleNameRepository
    {
        Task<IEnumerable<FemaleName>> GetAllFemaleNamesAsync();
        
        Task<FemaleName?> GetFemaleNameByIdAsync(int id);
        
        Task<FemaleName> CreateFemaleNameAsync(FemaleName name);
        
        Task<FemaleName?> UpdateFemaleNameAsync(int id, FemaleName updatedName);
        
        Task<bool> DeleteFemaleNameAsync(int id);
    }
}
