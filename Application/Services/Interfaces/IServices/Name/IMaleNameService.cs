using Application.DtoModels.Name.MaleName;

namespace Application.Services.Interfaces.IServices.Name
{
    public interface IMaleNameService
    {
        Task<IEnumerable<MaleNameDto>> GetAllMaleNamesAsync();
        
        Task<MaleNameDto?> GetMaleNameByIdAsync(int id);
        
        Task<MaleNameDto?> CreateMaleNameAsync(CreateMaleNameDto dto);
        
        Task<MaleNameDto?> UpdateMaleNameAsync(int id, UpdateMaleNameDto dto);
        
        Task<bool> DeleteMaleNameAsync(int id);
    }
}
