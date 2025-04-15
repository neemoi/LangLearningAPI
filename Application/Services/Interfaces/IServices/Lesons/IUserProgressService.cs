using Application.DtoModels.Lessons.Progress;

namespace Application.Services.Interfaces.IServices.Lesons
{
    public interface IUserProgressService
    {
        Task<UserProgressCreateUpdateDto> AddUserProgressAsync(UserProgressCreateUpdateDto progressDto);
        
        Task<UserProgressCreateUpdateDto> UpdateUserProgressAsync(UserProgressCreateUpdateDto progressDto);
        
        Task<UserLessonProgressViewDto> GetFullProgressAsync(string userId, int lessonId);
        
        Task<UserWordStatsDto> GetWordStatsAsync(string userId, int lessonId);

        Task<UserWordProgressResponseDto> AddWordProgressAsync(UserWordProgressCreateDto progressDto);
        
        Task<UserWordProgressResponseDto> UpdateWordProgressAsync(int id, UserWordProgressCreateDto progressDto);
        
        Task<UserWordProgressResponseDto> GetWordProgressAsync(int id);
        
        Task<List<UserWordProgressResponseDto>> GetWordProgressesByUserAndLessonAsync(string userId, int lessonId);
    }
}