using Application.DtoModels.Lessons.Progress;
using Domain.Models;

namespace Application.Services.Interfaces.IServices.Lesons
{
    public interface IUserProgressRepository
    {
        Task<UserProgress> AddUserProgressAsync(UserProgress userProgress);
        
        Task<UserProgress> UpdateUserProgressAsync(UserProgress userProgress);
        
        Task<UserLessonProgressViewDto> GetFullProgressAsync(string userId, int lessonId);
        
        Task<UserWordStatsDto> GetWordStatsAsync(string userId, int lessonId);

        Task<Domain.Models.UserWordProgress> AddWordProgressAsync(Domain.Models.UserWordProgress progress);
        
        Task<Domain.Models.UserWordProgress> UpdateWordProgressAsync(Domain.Models.UserWordProgress progress);
        
        Task<Domain.Models.UserWordProgress> GetWordProgressAsync(int id);
       
        Task<List<Domain.Models.UserWordProgress>> GetWordProgressesByUserAndLessonAsync(string userId, int lessonId);
    }
}