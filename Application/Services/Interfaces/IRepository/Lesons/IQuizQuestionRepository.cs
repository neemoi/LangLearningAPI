using Application.DtoModels.Lessons.QuizQuestion;
using Domain.Models;

namespace Application.Services.Interfaces.IRepository.Lesons
{
    public interface IQuizQuestionRepository
    {
        Task<QuizQuestion> CreateQuizQuestionAsync(QuizQuestion entity);
        
        Task<QuizQuestion?> GetQuizQuestionByIdAsync(int id);
        
        Task<IEnumerable<QuizQuestion>> GetQuizQuestionAllAsync();

        Task<QuizQuestion> UpdateQuizQuestionAsync(UpdateQuizQuestionDto dto);

        Task<QuizQuestion> DeleteQuizQuestionAsync(int id);
        
        Task<IEnumerable<QuizQuestion>> GetByQuizIdAsync(int quizId);

        Task<bool> QuizExistsAsync(int quizId);
    }
}
