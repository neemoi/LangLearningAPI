using Application.DtoModels.Name.EnglishName;
using Application.Services.Interfaces.IServices.Name;
using Application.UnitOfWork;
using AutoMapper;
using Domain.Models;
using Microsoft.Extensions.Logging;

namespace Application.Services.Implementations.Name
{
    public class NameService : INameService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger<NameService> _logger;

        public NameService(IUnitOfWork unitOfWork, IMapper mapper, ILogger<NameService> logger)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<IEnumerable<EnglishNameDto>> GetAllEnglishNamesAsync()
        {
            try
            {
                var entities = await _unitOfWork.EnglishNameRepository.GetAllEnglishNamesAsync();
               
                return _mapper.Map<IEnumerable<EnglishNameDto>>(entities);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to get all English names.");
                return Enumerable.Empty<EnglishNameDto>();
            }
        }

        public async Task<EnglishNameDto?> GetEnglishNameByIdAsync(int id)
        {
            try
            {
                var entity = await _unitOfWork.EnglishNameRepository.GetEnglishNameByIdAsync(id);
                
                return entity == null ? null : _mapper.Map<EnglishNameDto>(entity);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Failed to get English name with id {id}.");
                return null;
            }
        }

        public async Task<EnglishNameDto?> CreateEnglishNameAsync(CreateEnglishNameDto dto)
        {
            try
            {
                var entity = _mapper.Map<EnglishName>(dto);
                var result = await _unitOfWork.EnglishNameRepository.CreateEnglishNameAsync(entity);
                
                return result == null ? null : _mapper.Map<EnglishNameDto>(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to create English name.");
                return null;
            }
        }

        public async Task<EnglishNameDto?> UpdateEnglishNameAsync(int id, UpdateEnglishNameDto dto)
        {
            try
            {
                var existing = await _unitOfWork.EnglishNameRepository.GetEnglishNameByIdAsync(id);
                if (existing == null)
                {
                    _logger.LogWarning($"English name with ID {id} not found.");
                    return null;
                }

                if (dto.Name != null)
                    existing.Name = dto.Name;

                if (dto.ImagePath != null)
                    existing.ImagePath = dto.ImagePath;

                var updated = await _unitOfWork.EnglishNameRepository.UpdateEnglishNameAsync(id, existing);
                
                return updated == null ? null : _mapper.Map<EnglishNameDto>(updated);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Failed to update English name with id {id}.");
                return null;
            }
        }

        public async Task<bool> DeleteEnglishNameAsync(int id)
        {
            try
            {
                return await _unitOfWork.EnglishNameRepository.DeleteEnglishNameAsync(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Failed to delete English name with id {id}.");
                return false;
            }
        }
    }
}
