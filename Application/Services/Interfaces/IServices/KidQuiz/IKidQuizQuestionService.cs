using Application.DtoModels.KidQuiz;
using Application.DtoModels.KidQuiz.KidQuizQuestion;
using Domain.Models;

namespace Application.Services.Interfaces.IServices.KidQuiz
{
    public interface IKidQuizQuestionService
    {
        Task<KidQuizQuestionDto?> GetByIdQuizQuestionAsync(int id);

        Task<IEnumerable<KidQuizQuestionDto>> GetByLessonIdQuizQuestionAsync(int lessonId);

        Task<KidQuizQuestionDto?> AddQuizQuestionAsync(CreateKidQuizQuestionDto questionDto);

        Task<KidQuizQuestionDto?> UpdateQuizQuestionAsync(int id, UpdateKidQuizQuestionDto questionDto);

        Task<KidQuizQuestion?> DeleteQuizQuestionAsync(int id);
    }
}
