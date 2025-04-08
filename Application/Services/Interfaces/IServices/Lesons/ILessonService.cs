using Application.DtoModels.Lessons.Lessons;
using Application.DtoModels.Lessons.Quiz;

namespace Application.Services.Interfaces.IServices.Lesons
{
    public interface ILessonService
    {
        Task<LessonDto?> GetLessonByIdAsync(int id);

        Task<List<LessonDto>> GetAllLessonsAsync();

        Task<LessonDto> CreateLessonAsync(CreateLessonDto createDto);

        Task<LessonDto> UpdateLessonAsync(UpdateLessonDto updateDto);

        Task<LessonDto?> DeleteLessonAsync(int id);

        Task<List<LessonWordDto>> GetLessonWordsAsync(int lessonId);

        Task<List<LessonPhraseDto>> GetLessonPhrasesAsync(int lessonId);

        Task<List<QuizDto>> GetLessonQuizzesAsync(int lessonId);

        Task<LessonDetailDto> GetLessonDetailAsync(int id);
    }
}
