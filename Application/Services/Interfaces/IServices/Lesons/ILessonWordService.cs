using Application.DtoModels.Lessons.Words;

namespace Application.Services.Interfaces.IServices.Lesons
{
    public interface ILessonWordService
    {
        Task<IEnumerable<LessonWordDto>> GetAllWordsAsync();

        Task<LessonWordDto?> GetWordByIdAsync(int id);

        Task<LessonWordDto> CreateWordAsync(CreateLessonWordDto dto);

        Task<LessonWordDto> UpdatePartialWordsAsync(int id, UpdateLessonWordDto dto);

        Task<LessonWordDto> DeleteWordsAsync(int id);
    }
}