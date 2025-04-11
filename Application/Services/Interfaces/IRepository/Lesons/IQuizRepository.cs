using Domain.Models;

namespace Application.Services.Interfaces.IRepository.Lesons
{
    public interface IQuizRepository
    {
        Task<Quiz> CreateQuizAsync(Quiz entity);
        
        Task<Quiz> GetQuizByIdAsync(int id);
        
        Task<IEnumerable<Quiz>> GetAllQuizzesAsync();
        
        Task<Quiz> UpdateQuizAsync(Quiz entity);
        
        Task<Quiz> DeleteQuizAsync(int id);

        Task<bool> QuizExistsInLessonAsync(int lessonId, string type);

        Task<IEnumerable<Quiz>> GetQuizzesByLessonIdAsync(int lessonId);
    }
}
