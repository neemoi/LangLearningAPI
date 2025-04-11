using Application.DtoModels.Lessons.Quiz;

namespace Application.Services.Interfaces.IServices.Lesons
{
    public interface IQuizService
    {
        Task<QuizDto> CreateQuizAsync(CreateQuizDto dto);
        
        Task<QuizDto> GetQuizByIdAsync(int id);
        
        Task<IEnumerable<QuizDto>> GetAllQuizzesAsync();
        
        Task<QuizDto> UpdateQuizAsync(UpdateQuizDto dto);

        Task<QuizDto> DeleteQuizAsync(int id);
        
        Task<IEnumerable<QuizDto>> GetQuizzesByLessonIdAsync(int lessonId);
    }
}
