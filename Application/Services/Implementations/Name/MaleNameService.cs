using Application.DtoModels.Name.MaleName;
using Application.Services.Interfaces.IServices.Name;
using Application.UnitOfWork;
using AutoMapper;
using Domain.Models;
using Microsoft.Extensions.Logging;

namespace Application.Services.Implementations.Name
{
    public class MaleNameService : IMaleNameService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger<MaleNameService> _logger;

        public MaleNameService(IUnitOfWork unitOfWork, IMapper mapper, ILogger<MaleNameService> logger)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<IEnumerable<MaleNameDto>> GetAllMaleNamesAsync()
        {
            try
            {
                var entities = await _unitOfWork.MaleNameRepository.GetAllMaleNamesAsync();
                
                return _mapper.Map<IEnumerable<MaleNameDto>>(entities);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to get all male names.");
                return Enumerable.Empty<MaleNameDto>();
            }
        }

        public async Task<MaleNameDto?> GetMaleNameByIdAsync(int id)
        {
            try
            {
                var entity = await _unitOfWork.MaleNameRepository.GetMaleNameByIdAsync(id);
                
                return entity == null ? null : _mapper.Map<MaleNameDto>(entity);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Failed to get male name with id {id}.");
                return null;
            }
        }

        public async Task<MaleNameDto?> CreateMaleNameAsync(CreateMaleNameDto dto)
        {
            try
            {
                var entity = _mapper.Map<MaleName>(dto);
                var result = await _unitOfWork.MaleNameRepository.CreateMaleNameAsync(entity);
                
                return result == null ? null : _mapper.Map<MaleNameDto>(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to create male name.");
                return null;
            }
        }

        public async Task<MaleNameDto?> UpdateMaleNameAsync(int id, UpdateMaleNameDto dto)
        {
            try
            {
                var existing = await _unitOfWork.MaleNameRepository.GetMaleNameByIdAsync(id);
                if (existing == null)
                {
                    _logger.LogWarning("Male name with ID {Id} not found", id);
                    return null;
                }

                if (dto.Name != null)
                    existing.Name = dto.Name;

                if (dto.EnglishNameId > 0)
                    existing.EnglishNameId = dto.EnglishNameId;

                var updated = await _unitOfWork.MaleNameRepository.UpdateMaleNameAsync(id, existing);
                
                return updated == null ? null : _mapper.Map<MaleNameDto>(updated);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Failed to update male name with id {id}.");
                return null;
            }
        }

        public async Task<bool> DeleteMaleNameAsync(int id)
        {
            try
            {
                return await _unitOfWork.MaleNameRepository.DeleteMaleNameAsync(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Failed to delete male name with id {id}.");
                return false;
            }
        }
    }
}