using Application.DtoModels.Lessons.Phrasees;

namespace Application.Services.Interfaces.IServices.Lesons
{
    public interface ILessonPhraseService
    {
        Task<LessonPhraseDto> CreatePhraseAsync(CreateLessonPhraseDto dto);
        
        Task<IEnumerable<LessonPhraseDto>> GetAllPhrasesAsync();
        
        Task<LessonPhraseDto> GetPhraseByIdAsync(int id);

        Task<LessonPhraseDto> UpdatePhraseAsync(UpdateLessonPhraseDto dto);

        Task<LessonPhraseDto> DeletePhraseAsync(int id);
    }
}
