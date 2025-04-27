using Application.DtoModels.KidQuiz.KidQuizAnswer;

namespace Application.Services.Interfaces.IServices.KidQuiz
{
    public interface IKidQuizAnswerService
    {
        Task<KidQuizAnswerDto?> GetKidQuizAnswerByIdAsync(int id);
        
        Task<IEnumerable<KidQuizAnswerDto>> GetAnswersByQuestionIdAsync(int questionId);
        
        Task<KidQuizAnswerDto?> AddKidQuizAnswerAsync(CreateKidQuizAnswerDto dto);
        
        Task<KidQuizAnswerDto?> UpdateKidQuizAnswerAsync(UpdateKidQuizAnswerDto dto);
        
        Task<bool> DeleteKidQuizAnswerAsync(int id);
    }
}
