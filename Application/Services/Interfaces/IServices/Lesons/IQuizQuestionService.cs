using Application.DtoModels.Lessons.QuizQuestion;
using Domain.Models;

namespace Application.Services.Interfaces.IServices.Lesons
{
    public interface IQuizQuestionService
    {

        Task<QuizQuestionDto> CreateQuizQuestionAsync(CreateQuizQuestionDto dto);
        
        Task<QuizQuestionDto> GetQuizQuestionByIdAsync(int id);
        
        Task<IEnumerable<QuizQuestionDto>> GetAllQuizQuestionsAsync();

        Task<QuizQuestion> UpdateQuizQuestionAsync(UpdateQuizQuestionDto dto);

        Task DeleteQuizQuestionAsync(int id);
        
        Task<IEnumerable<QuizQuestionDto>> GetQuizQuestionsByQuizIdAsync(int quizId);
    }
}
