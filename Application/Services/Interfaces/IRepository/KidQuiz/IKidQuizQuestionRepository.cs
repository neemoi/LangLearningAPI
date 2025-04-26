using Application.DtoModels.KidQuiz;
using Domain.Models;

namespace Application.Services.Interfaces.IRepository.KidQuiz
{
    public interface IKidQuizQuestionRepository
    {
        Task<KidQuizQuestion?> GetByIdQuizQuestionAsync(int id);

        Task<IEnumerable<KidQuizQuestionDto>> GetByLessonIdQuizQuestionAsync(int lessonId);

        Task<KidQuizQuestion?> AddQuizQuestionAsync(KidQuizQuestion question);

        Task<KidQuizQuestion?> UpdateQuizQuestionAsync(KidQuizQuestion question);

        Task<KidQuizQuestion?> DeleteQuizQuestionAstnc(KidQuizQuestion question);
    }
}
