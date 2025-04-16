using Application.DtoModels.Nouns;
using Domain.Models;

namespace Application.Services.Interfaces.IServices.Nouns
{
    public interface IAlphabetLetterService
    {
        Task<IEnumerable<AlphabetLetterDto>> GetAllLettersAsync();
        
        Task<AlphabetLetterDto?> GetLetterByIdAsync(int id);
        
        Task<AlphabetLetterDto> AddLetterAsync(CreateAlphabetLetterDto letterDto);
        
        Task<AlphabetLetterDto> UpdateLetterAsync(UpdateAlphabetLetterDto letterDto);
        
        Task<bool> DeleteLetterAsync(int id);
    }
}
