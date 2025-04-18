using Domain.Models;

namespace Application.Services.Interfaces.IRepository.Functions
{
    public interface IFunctionWordRepository
    {
        Task<FunctionWord> GetByIdFunctionWordAsync(int id);
        
        Task<IEnumerable<FunctionWord>> GetAllFunctionWordAsync();

        Task<FunctionWord> AddFunctionWordAsync(FunctionWord entity);

        Task UpdateFunctionWordAsync(FunctionWord entity);

        Task<bool> DeleteFunctionWordAsync(int id);
        
        Task<bool> ExistsFunctionWordAsync(int id);
    }
}
