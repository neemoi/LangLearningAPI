using Application.DtoModels.KidQuiz.Lessons;
using Domain.Models;

namespace Application.Services.Interfaces.IRepository.KidQuiz
{
    public interface IKidLessonRepository
    {
        Task<List<KidLessonDto>> GetAllLessonsAsync();

        Task<KidLesson?> GetLessonByIdAsync(int id);

        Task<KidLessonDto?> CreateLessonAsync(KidLesson lesson);

        Task<KidLesson?> UpdateLessonAsync(int id, KidLesson lesson);

        Task<KidLessonDto?> DeleteLessonAsync(int id);
    }
}
