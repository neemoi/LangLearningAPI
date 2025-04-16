using Application.DtoModels.Nouns;
using Application.Services.Interfaces.IRepository.Nouns;
using AutoMapper;
using Domain.Models;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

public class AlphabetLetterRepository : IAlphabetLetterRepository
{
    private readonly LanguageLearningDbContext _context;
    private readonly ILogger<AlphabetLetterRepository> _logger;
    private readonly IMapper _mapper;

    public AlphabetLetterRepository(LanguageLearningDbContext context, ILogger<AlphabetLetterRepository> logger, IMapper mapper)
    {
        _context = context;
        _logger = logger;
        _mapper = mapper;
    }

    public async Task<IEnumerable<AlphabetLetterDto>> GetAllAlphabetLetterAsync()
    {
        try
        {
            var letters = await _context.AlphabetLetters
                .Include(l => l.Words)
                .ToListAsync();

            return _mapper.Map<IEnumerable<AlphabetLetterDto>>(letters);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while getting all alphabet letters");
            throw;
        }
    }

    public async Task<AlphabetLetterDto?> GetAlphabetLetterByIdAsync(int id)
    {
        try
        {
            var letter = await _context.AlphabetLetters
                .Include(l => l.Words)
                .FirstOrDefaultAsync(l => l.Id == id);

            return letter == null ? null : _mapper.Map<AlphabetLetterDto>(letter);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error occurred while getting alphabet letter with id {id}");
            throw;
        }
    }

    public async Task<AlphabetLetterDto> AddAlphabetLetterAsync(CreateAlphabetLetterDto letterDto)
    {
        try
        {
            var existingLetter = await _context.AlphabetLetters
                .FirstOrDefaultAsync(l => l.Symbol.ToLower() == letterDto.Symbol.ToLower());

            if (existingLetter != null)
                throw new ArgumentException($"Letter '{letterDto.Symbol}' already exists.");

            var letter = _mapper.Map<AlphabetLetter>(letterDto);
            await _context.AlphabetLetters.AddAsync(letter);
            await _context.SaveChangesAsync();

            return _mapper.Map<AlphabetLetterDto>(letter);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while adding alphabet letter");
            throw;
        }
    }


    public async Task<AlphabetLetter> UpdateAlphabetLetterAsync(UpdateAlphabetLetterDto letterDto)
    {
        try
        {
            var existingLetter = await _context.AlphabetLetters.FindAsync(letterDto.Id);
            if (existingLetter == null)
                throw new KeyNotFoundException($"Alphabet letter with id {letterDto.Id} not found");

            if (letterDto.Symbol != null)
                existingLetter.Symbol = letterDto.Symbol;

            if (letterDto.ImageUrl != null)
                existingLetter.ImageUrl = letterDto.ImageUrl;

            await _context.SaveChangesAsync();
            
            return existingLetter;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error occurred while updating alphabet letter with id {letterDto.Id}");
            throw;
        }
    }

    public async Task<bool> DeleteAlphabetLetterAsync(int id)
    {
        try
        {
            var letter = await _context.AlphabetLetters.FindAsync(id);
            if (letter == null)
                return false;

            _context.AlphabetLetters.Remove(letter);
            await _context.SaveChangesAsync();
            
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error occurred while deleting alphabet letter with id {id}");
            throw;
        }
    }
}
