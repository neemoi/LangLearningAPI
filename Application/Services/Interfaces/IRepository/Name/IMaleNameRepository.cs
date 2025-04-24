using Domain.Models;

namespace Application.Services.Interfaces.IRepository.Name
{
    public interface IMaleNameRepository
    {
        Task<IEnumerable<MaleName>> GetAllMaleNamesAsync();
        
        Task<MaleName?> GetMaleNameByIdAsync(int id);
        
        Task<MaleName?> CreateMaleNameAsync(MaleName name);
        
        Task<MaleName?> UpdateMaleNameAsync(int id, MaleName updatedName);
        
        Task<bool> DeleteMaleNameAsync(int id);
    }
}
