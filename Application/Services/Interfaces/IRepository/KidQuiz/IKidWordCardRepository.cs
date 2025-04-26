using Domain.Models;

namespace Application.Services.Interfaces.IRepository.KidQuiz
{
    public interface IKidWordCardRepository
    {
        Task<KidWordCard?> GetByIdAsync(int id);
        
        Task<IEnumerable<KidWordCard>> GetAllAsync();
        
        Task<KidWordCard> AddAsync(KidWordCard wordCard);
        
        Task<KidWordCard?> UpdateAsync(KidWordCard wordCard);
        
        Task<bool> DeleteAsync(int id);
        
        Task<IEnumerable<KidWordCard>> GetWordCardsByLessonIdAsync(int lessonId);
    }
}
