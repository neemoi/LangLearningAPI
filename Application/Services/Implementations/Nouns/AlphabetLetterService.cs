using Application.DtoModels.Nouns;
using Application.Services.Interfaces.IServices.Nouns;
using Application.UnitOfWork;
using AutoMapper;
using Microsoft.Extensions.Logging;

namespace Application.Services.Implementations.Nouns
{
    public class AlphabetLetterService : IAlphabetLetterService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<AlphabetLetterService> _logger;
        private readonly IMapper _mapper;

        public AlphabetLetterService(IUnitOfWork unitOfWork, ILogger<AlphabetLetterService> logger, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task<IEnumerable<AlphabetLetterDto>> GetAllLettersAsync()
        {
            try
            {
                var letters = await _unitOfWork.AlphabetLetterRepository.GetAllAlphabetLetterAsync();
                return _mapper.Map<IEnumerable<AlphabetLetterDto>>(letters);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while getting all letters");
                throw;
            }
        }

        public async Task<AlphabetLetterDto?> GetLetterByIdAsync(int id)
        {
            try
            {
                var letter = await _unitOfWork.AlphabetLetterRepository.GetAlphabetLetterByIdAsync(id);
                return _mapper.Map<AlphabetLetterDto>(letter);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error occurred while getting letter with id {id}");
                throw;
            }
        }

        public async Task<AlphabetLetterDto> AddLetterAsync(CreateAlphabetLetterDto letterDto)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(letterDto.Symbol))
                    throw new ArgumentException("Symbol cannot be empty");

                if (string.IsNullOrWhiteSpace(letterDto.ImageUrl))
                    throw new ArgumentException("Image URL cannot be empty");

                var createdLetter = await _unitOfWork.AlphabetLetterRepository.AddAlphabetLetterAsync(letterDto);
                return _mapper.Map<AlphabetLetterDto>(createdLetter);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while adding letter");
                throw;
            }
        }

        public async Task<AlphabetLetterDto> UpdateLetterAsync(UpdateAlphabetLetterDto letterDto)
        {
            try
            {
                if (letterDto.Id <= 0)
                    throw new ArgumentException("Id must be provided for update");

                var existing = await _unitOfWork.AlphabetLetterRepository.GetAlphabetLetterByIdAsync(letterDto.Id);
                if (existing == null)
                    throw new KeyNotFoundException($"Letter with id {letterDto.Id} not found");

                if (!string.IsNullOrWhiteSpace(letterDto.Symbol) && letterDto.Symbol.Length != 1)
                    throw new ArgumentException("Symbol must be exactly 1 character");

                if (!string.IsNullOrWhiteSpace(letterDto.ImageUrl) && !Uri.IsWellFormedUriString(letterDto.ImageUrl, UriKind.Absolute))
                    throw new ArgumentException("Invalid image URL");

                var updated = await _unitOfWork.AlphabetLetterRepository.UpdateAlphabetLetterAsync(letterDto);

                return new AlphabetLetterDto
                {
                    Id = updated.Id,
                    Symbol = updated.Symbol,
                    ImageUrl = updated.ImageUrl
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error occurred while updating letter with id {letterDto.Id}");
                throw;
            }
        }

        public async Task<bool> DeleteLetterAsync(int id)
        {
            try
            {
                var letter = await _unitOfWork.AlphabetLetterRepository.GetAlphabetLetterByIdAsync(id);
                if (letter == null)
                    return false;

                return await _unitOfWork.AlphabetLetterRepository.DeleteAlphabetLetterAsync(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error occurred while deleting letter with id {id}");
                throw;
            }
        }
    }
}
