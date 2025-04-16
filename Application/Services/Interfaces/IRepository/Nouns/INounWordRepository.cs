using Domain.Models;

namespace Application.Services.Interfaces.IRepository.Nouns
{
    public interface INounWordRepository
    {
        Task<IEnumerable<NounWord>> GetAllNounWordAsync();

        Task<NounWord?> GetNounWordByIdAsync(int id);

        Task<IEnumerable<NounWord>> GetByLetterIdAsync(int letterId);

        Task<NounWord> AddNounWordAsync(NounWord word);

        Task<NounWord?> UpdateNounWordAsync(NounWord word);

        Task<bool> DeleteNounWordAsync(int id);
    }
}