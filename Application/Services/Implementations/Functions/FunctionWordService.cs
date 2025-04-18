using Application.DtoModels.Functions;
using Application.Services.Interfaces.IServices.Functions;
using Application.UnitOfWork;
using AutoMapper;
using Domain.Models;
using Microsoft.Extensions.Logging;

public class FunctionWordService : IFunctionWordService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<FunctionWordService> _logger;
    private readonly IMapper _mapper;

    public FunctionWordService(IUnitOfWork unitOfWork, ILogger<FunctionWordService> logger,
        IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
        _mapper = mapper;
    }

    public async Task<FunctionWordDto?> GetFunctionWordByIdAsync(int id)
    {
        try
        {
            var entity = await _unitOfWork.FunctionWordRepository.GetByIdFunctionWordAsync(id);
            if (entity == null)
                return null;

            return _mapper.Map<FunctionWordDto>(entity);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error when receiving Function Word with ID {Id}", id);
            throw;
        }
    }

    public async Task<IEnumerable<FunctionWordDto>> GetAllFunctionWordAsync()
    {
        try
        {
            var entities = await _unitOfWork.FunctionWordRepository.GetAllFunctionWordAsync();
            
            return _mapper.Map<IEnumerable<FunctionWordDto>>(entities);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error when getting all FunctionWords");
            throw;
        }
    }

    public async Task<FunctionWordDto> AddFunctionWordAsync(CreateFunctionWordDto dto)
    {
        try
        {
            var functionWord = _mapper.Map<FunctionWord>(dto);
            await _unitOfWork.FunctionWordRepository.AddFunctionWordAsync(functionWord);
            
            return _mapper.Map<FunctionWordDto>(functionWord);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error while adding FunctionWord");
            throw;
        }
    }

    public async Task<bool> DeleteFunctionWordAsync(int id)
    {
        try
        {
            return await _unitOfWork.FunctionWordRepository.DeleteFunctionWordAsync(id);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting Function Word from ID {Id}", id);
            throw;
        }
    }

    public async Task<bool> ExistsFunctionWordAsync(int id)
    {
        try
        {
            return await _unitOfWork.FunctionWordRepository.ExistsFunctionWordAsync(id);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error checking existence of Function Word from ID {Id}", id);
            throw;
        }
    }

    public async Task<FunctionWordDto> UpdateFunctionWordAsync(FunctionWordUpdateDto dto)
    {
        try
        {
            var existingEntity = await _unitOfWork.FunctionWordRepository.GetByIdFunctionWordAsync(dto.Id);
            if (existingEntity == null)
            {
                _logger.LogWarning("Function Word with ID {Id} not found for update", dto.Id);
                throw new KeyNotFoundException($"FunctionWord with ID {dto.Id} not found");
            }

            if (!string.IsNullOrWhiteSpace(dto.Word))
                existingEntity.Name = dto.Word;

            await _unitOfWork.FunctionWordRepository.UpdateFunctionWordAsync(existingEntity);

            return _mapper.Map<FunctionWordDto>(existingEntity);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating Function Word with ID {Id}", dto.Id);
            throw;
        }
    }
}
