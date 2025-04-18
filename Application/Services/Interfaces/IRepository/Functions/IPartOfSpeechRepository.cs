using Domain.Models;

namespace Application.Services.Interfaces.IRepository.Functions
{
    public interface IPartOfSpeechRepository
    {
        Task<PartOfSpeech?> GetByIPartOfSpeechdAsync(int id);

        Task<IEnumerable<PartOfSpeech>> GetAllPartOfSpeechAsync();

        Task<PartOfSpeech> AddPartOfSpeechAsync(PartOfSpeech partOfSpeech);

        Task UpdatePartOfSpeechAsync(PartOfSpeech partOfSpeech);

        Task<bool> DeletePartOfSpeechAsync(int id);
    }

}
