using Application.DtoModels.KidQuiz.KidWordCard;
using Application.DtoModels.KidQuiz;

namespace Application.Services.Interfaces.IServices.KidQuiz
{
    public interface IKidWordCardService
    {
        Task<KidWordCardDto?> GetWordCardByIdAsync(int id);
        
        Task<IEnumerable<KidWordCardDto>> GetAllWordCardsAsync();
        
        Task<KidWordCardDto?> CreateWordCardAsync(CreateKidWordCardDto dto);
        
        Task<KidWordCardDto?> UpdateWordCardAsync(int id, UpdateKidWordCardDto dto);
        
        Task<bool> DeleteWordCardAsync(int id);
        
        Task<IEnumerable<KidWordCardDto>> GetWordCardsByLessonIdAsync(int lessonId);
    }
}
