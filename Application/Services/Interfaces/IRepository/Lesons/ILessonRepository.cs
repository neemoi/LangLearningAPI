using Domain.Models;

namespace Application.Services.Interfaces.IRepository.Lesons
{
    public interface ILessonRepository
    {
        Task<Lesson?> GetLessonByIdAsync(int id);

        Task<List<Lesson>> GetAllLessonsAsync();

        Task<Lesson> AddLessonAsync(Lesson lesson);

        Task<Lesson> UpdateLessonAsync(int id, Action<Lesson> updateAction);

        Task<Lesson?> DeleteLessonAsync(int id);

        Task<List<LessonWord>> GetWordsByLessonIdAsync(int lessonId);

        Task<List<LessonPhrase>> GetPhrasesByLessonAsync(int lessonId);

        Task<List<Quiz>> GetQuizzesByLessonAsync(int lessonId);
    }
}