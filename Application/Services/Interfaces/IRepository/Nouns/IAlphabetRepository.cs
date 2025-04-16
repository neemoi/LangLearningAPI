using Application.DtoModels.Nouns;
using Domain.Models;

namespace Application.Services.Interfaces.IRepository.Nouns
{
    public interface IAlphabetLetterRepository
    {
        Task<IEnumerable<AlphabetLetterDto>> GetAllAlphabetLetterAsync();
        
        Task<AlphabetLetterDto?> GetAlphabetLetterByIdAsync(int id);
        
        Task<AlphabetLetterDto> AddAlphabetLetterAsync(CreateAlphabetLetterDto letterDto);

        Task<AlphabetLetter> UpdateAlphabetLetterAsync(UpdateAlphabetLetterDto letterDto);

        Task<bool> DeleteAlphabetLetterAsync(int id);
    }

}
