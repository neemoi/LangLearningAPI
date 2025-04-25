using Application.DtoModels.KidQuiz.Lessons;

namespace Application.Services.Interfaces.IServices.KidQuiz
{
    public interface IKidLessonService
    {
        Task<List<KidLessonDto>> GetAllLessonsAsync();
        
        Task<KidLessonDto?> GetLessonByIdAsync(int id);
        
        Task<KidLessonDto?> CreateLessonAsync(CreateKidLessonDto dto);

        Task<KidLessonDto?> UpdateLessonAsync(int id, UpdateKidLessonDto dto);

        Task<KidLessonDto?> DeleteLessonAsync(int id);
    }
}
