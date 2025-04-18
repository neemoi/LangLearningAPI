using Application.DtoModels.Functions;

namespace Application.Services.Interfaces.IServices.Functions
{
    public interface IFunctionWordService
    {
        Task<FunctionWordDto?> GetFunctionWordByIdAsync(int id);

        Task<IEnumerable<FunctionWordDto>> GetAllFunctionWordAsync();

        Task<FunctionWordDto> AddFunctionWordAsync(CreateFunctionWordDto dto);

        Task<FunctionWordDto> UpdateFunctionWordAsync(FunctionWordUpdateDto dto);

        Task<bool> DeleteFunctionWordAsync(int id);

        Task<bool> ExistsFunctionWordAsync(int id);
    }
}
