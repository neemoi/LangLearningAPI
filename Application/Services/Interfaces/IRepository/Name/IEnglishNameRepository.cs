using Domain.Models;

namespace Application.Services.Interfaces.IRepository.Name
{
    public interface IEnglishNameRepository
    {
        Task<IEnumerable<EnglishName>> GetAllEnglishNamesAsync();
        
        Task<EnglishName?> GetEnglishNameByIdAsync(int id);
        
        Task<EnglishName?> CreateEnglishNameAsync(EnglishName name);
        
        Task<EnglishName?> UpdateEnglishNameAsync(int id, EnglishName updatedName);
        
        Task<bool> DeleteEnglishNameAsync(int id);
    }
}
