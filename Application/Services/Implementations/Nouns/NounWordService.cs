using Application.DtoModels.Nouns;
using Application.Services.Interfaces.IServices.Nouns;
using Application.UnitOfWork;
using AutoMapper;
using Domain.Models;
using Microsoft.Extensions.Logging;

public class NounWordService : INounWordService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<NounWordService> _logger;
    private readonly IMapper _mapper;

    public NounWordService(IUnitOfWork unitOfWork, ILogger<NounWordService> logger, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
        _mapper = mapper;
    }

    public async Task<IEnumerable<NounWordDto>> GetAllWordsAsync()
    {
        try
        {
            var words = await _unitOfWork.NounWordRepository.GetAllNounWordAsync();
            
            return _mapper.Map<IEnumerable<NounWordDto>>(words);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting all words");
            throw;
        }
    }

    public async Task<NounWordDto?> GetWordByIdAsync(int id)
    {
        try
        {
            var word = await _unitOfWork.NounWordRepository.GetNounWordByIdAsync(id);
            
            return word == null ? null : _mapper.Map<NounWordDto>(word);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error getting word with id {id}");
            throw;
        }
    }

    public async Task<IEnumerable<NounWordDto>> GetWordsByLetterIdAsync(int letterId)
    {
        try
        {
            var words = await _unitOfWork.NounWordRepository.GetByLetterIdAsync(letterId);
            
            return _mapper.Map<IEnumerable<NounWordDto>>(words);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error getting words for letter id {letterId}");
            throw;
        }
    }

    public async Task<NounWordDto> AddWordAsync(NounWordDto wordDto)
    {
        try
        {
            ValidateDto(wordDto);

            var letterExists = await _unitOfWork.AlphabetLetterRepository
                .GetAlphabetLetterByIdAsync(wordDto.AlphabetLetterId);

            if (letterExists == null)
                throw new KeyNotFoundException($"Letter with id {wordDto.AlphabetLetterId} not found");

            var word = _mapper.Map<NounWord>(wordDto);
            await _unitOfWork.NounWordRepository.AddNounWordAsync(word);

            return _mapper.Map<NounWordDto>(word);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error adding word: {@WordDto}", wordDto);
            throw;
        }
    }

    public async Task<NounWordDto> UpdateWordAsync(UpdateNounWordDto wordDto)
    {
        try
        {
            var existingWord = await _unitOfWork.NounWordRepository.GetNounWordByIdAsync(wordDto.Id);
            if (existingWord == null)
                throw new KeyNotFoundException($"Word with id {wordDto.Id} not found");

            if (wordDto.Name != null)
            {
                if (string.IsNullOrWhiteSpace(wordDto.Name))
                    throw new ArgumentException("Name cannot be empty");
                existingWord.Name = wordDto.Name;
            }

            if (wordDto.ImageUrl != null)
            {
                if (string.IsNullOrWhiteSpace(wordDto.ImageUrl))
                    throw new ArgumentException("Image URL cannot be empty");
                existingWord.ImageUrl = wordDto.ImageUrl;
            }

            if (wordDto.AlphabetLetterId.HasValue)
            {
                var letterExists = await _unitOfWork.AlphabetLetterRepository.GetAlphabetLetterByIdAsync(wordDto.AlphabetLetterId.Value);
                if (letterExists == null)
                    throw new KeyNotFoundException($"Letter with id {wordDto.AlphabetLetterId} not found");

                existingWord.AlphabetLetterId = wordDto.AlphabetLetterId.Value;
            }

            var updatedWord = await _unitOfWork.NounWordRepository.UpdateNounWordAsync(existingWord);

            return _mapper.Map<NounWordDto>(updatedWord);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error updating word with id {wordDto.Id}");
            throw;
        }
    }

    public async Task<bool> DeleteWordAsync(int id)
    {
        try
        {
            var word = await _unitOfWork.NounWordRepository.GetNounWordByIdAsync(id);
            if (word == null) return false;

            var result = await _unitOfWork.NounWordRepository.DeleteNounWordAsync(id);
            
            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error deleting word with id {id}");
            throw;
        }
    }

    private void ValidateDto(NounWordDto word)
    {
        if (string.IsNullOrWhiteSpace(word.Name))
            throw new ArgumentException("Name cannot be empty");

        if (string.IsNullOrWhiteSpace(word.ImageUrl))
            throw new ArgumentException("Image URL cannot be empty");
    }
}