using Application.DtoModels.Name.FemaleName;
using Application.Services.Interfaces.IServices.Name;
using Application.UnitOfWork;
using AutoMapper;
using Domain.Models;
using Microsoft.Extensions.Logging;

namespace Application.Services.Implementations.Name
{
    public class FemaleNameService : IFemaleNameService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger<FemaleNameService> _logger;

        public FemaleNameService(IUnitOfWork unitOfWork, IMapper mapper, ILogger<FemaleNameService> logger)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<IEnumerable<FemaleNameDto>> GetAllFemaleNamesAsync()
        {
            try
            {
                var entities = await _unitOfWork.FemaleNameRepository.GetAllFemaleNamesAsync();
                return _mapper.Map<IEnumerable<FemaleNameDto>>(entities);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to get all female names.");
                return Enumerable.Empty<FemaleNameDto>();
            }
        }

        public async Task<FemaleNameDto?> GetFemaleNameByIdAsync(int id)
        {
            try
            {
                var entity = await _unitOfWork.FemaleNameRepository.GetFemaleNameByIdAsync(id);
                if (entity == null)
                {
                    _logger.LogWarning("Female name with ID {Id} not found.", id);
                    return null;
                }

                return _mapper.Map<FemaleNameDto>(entity);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Failed to get female name with id {id}.");
                return null;
            }
        }

        public async Task<FemaleNameDto?> CreateFemaleNameAsync(CreateFemaleNameDto dto)
        {
            try
            {
                var englishName = await _unitOfWork.EnglishNameRepository.GetEnglishNameByIdAsync(dto.EnglishNameId);

                if (englishName == null)
                {
                    _logger.LogWarning("English name with ID {Id} not found", dto.EnglishNameId);
                    return null;
                }

                var femaleName = new FemaleName
                {
                    EnglishNameId = dto.EnglishNameId,
                    Name = dto.Name
                };

                var result = await _unitOfWork.FemaleNameRepository.CreateFemaleNameAsync(femaleName);

                var fullEntity = await _unitOfWork.FemaleNameRepository.GetFemaleNameByIdAsync(result.Id);

                return fullEntity == null ? null : _mapper.Map<FemaleNameDto>(fullEntity);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to create female name.");
                return null;
            }
        }

        public async Task<FemaleNameDto?> UpdateFemaleNameAsync(int id, UpdateFemaleNameDto dto)
        {
            try
            {
                var existing = await _unitOfWork.FemaleNameRepository.GetFemaleNameByIdAsync(id);
                if (existing == null)
                {
                    _logger.LogWarning("Female name with ID {Id} not found", id);
                    return null;
                }

                var englishName = await _unitOfWork.EnglishNameRepository.GetEnglishNameByIdAsync(dto.EnglishNameId);
                if (englishName == null)
                {
                    _logger.LogWarning("English name with ID {Id} not found", dto.EnglishNameId);
                    return null;
                }

                existing.Name = dto.Name;
                existing.EnglishNameId = dto.EnglishNameId;

                var updated = await _unitOfWork.FemaleNameRepository.UpdateFemaleNameAsync(id, existing);
                var fullEntity = await _unitOfWork.FemaleNameRepository.GetFemaleNameByIdAsync(updated.Id);

                return fullEntity == null ? null : _mapper.Map<FemaleNameDto>(fullEntity);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Failed to update female name with id {id}.");
                return null;
            }
        }

        public async Task<bool> DeleteFemaleNameAsync(int id)
        {
            try
            {
                return await _unitOfWork.FemaleNameRepository.DeleteFemaleNameAsync(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Failed to delete female name with id {id}.");
                return false;
            }
        }
    }
}
