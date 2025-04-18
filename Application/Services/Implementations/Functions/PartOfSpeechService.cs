using Application.DtoModels.Functions;
using Application.Services.Interfaces.IRepository.Functions;
using Application.Services.Interfaces.IServices.Functions;
using AutoMapper;
using Domain.Models;
using Microsoft.Extensions.Logging;


namespace Application.Services.Implementations.Functions
{
    public class PartOfSpeechService : IPartOfSpeechService
    {
        private readonly IPartOfSpeechRepository _repository;
        private readonly ILogger<PartOfSpeechService> _logger;
        private readonly IMapper _mapper;

        public PartOfSpeechService(IPartOfSpeechRepository repository, ILogger<PartOfSpeechService> logger, IMapper mapper)
        {
            _repository = repository;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task<PartOfSpeechDto?> GetPartOfSpeechByIdAsync(int id)
        {
            try
            {
                var entity = await _repository.GetByIPartOfSpeechdAsync(id);
                
                return entity != null ? _mapper.Map<PartOfSpeechDto>(entity) : null;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting PartOfSpeech with ID {Id}", id);
                throw;
            }
        }

        public async Task<IEnumerable<PartOfSpeechDto>> GetAllPartOfSpeechAsync()
        {
            try
            {
                var entities = await _repository.GetAllPartOfSpeechAsync();
                
                return _mapper.Map<IEnumerable<PartOfSpeechDto>>(entities);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting all PartOfSpeeches");
                throw;
            }
        }

        public async Task<PartOfSpeechDto> AddPartOfSpeechAsync(CreatePartOfSpeechDto dto)
        {
            try
            {
                var entity = _mapper.Map<PartOfSpeech>(dto);
                var added = await _repository.AddPartOfSpeechAsync(entity);
                
                return _mapper.Map<PartOfSpeechDto>(added);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding PartOfSpeech");
                throw;
            }
        }

        public async Task<PartOfSpeechDto> UpdatePartOfSpeechAsync(FunctionWordUpdateDto dto)
        {
            try
            {
                var existing = await _repository.GetByIPartOfSpeechdAsync(dto.Id);
                if (existing == null)
                {
                    _logger.LogWarning("PartOfSpeech with ID {Id} not found", dto.Id);
                    throw new KeyNotFoundException($"PartOfSpeech with ID {dto.Id} not found");
                }

                if (!string.IsNullOrWhiteSpace(dto.Word))
                    existing.Name = dto.Word;

                await _repository.UpdatePartOfSpeechAsync(existing);
                
                return _mapper.Map<PartOfSpeechDto>(existing);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating PartOfSpeech with ID {Id}", dto.Id);
                throw;
            }
        }

        public async Task<bool> DeletePartOfSpeechAsync(int id)
        {
            try
            {
                return await _repository.DeletePartOfSpeechAsync(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting PartOfSpeech with ID {Id}", id);
                throw;
            }
        }
    }
}
