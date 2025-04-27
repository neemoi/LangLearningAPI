using Domain.Models;

namespace Application.Services.Interfaces.IRepository.KidQuiz
{
    public interface IKidQuizAnswerRepository
    {
        Task<KidQuizAnswer?> GetByIdKidQuizAnswerAsync(int id);
        
        Task<IEnumerable<KidQuizAnswer>> GetByQuestionIdAsync(int questionId);
        
        Task<KidQuizAnswer> AddKidQuizAnswerAsync(KidQuizAnswer answer);
        
        Task<KidQuizAnswer> UpdateKidQuizAnswerAsync(KidQuizAnswer answer);
        
        Task<KidQuizAnswer> DeleteKidQuizAnswerAsync(int id);
    }
}
