using Application.DtoModels.Lessons.Phrasees;
using Domain.Models;

namespace Application.Services.Interfaces.IRepository.Lessons
{
    public interface ILessonPhraseRepository
    {
        Task<IEnumerable<LessonPhrase>> GetAllPhrasesAsync();
        
        Task<LessonPhrase?> GetPhraseByIdAsync(int id);
        
        Task<LessonPhrase> CreatePhraseAsync(LessonPhrase entity);

        Task<LessonPhrase> UpdatePhraseAsync(UpdateLessonPhraseDto dto);
        
        Task<LessonPhrase> DeletePhraseAsync(int id);
    }
}