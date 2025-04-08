using Application.DtoModels.Lessons.Words;
using Domain.Models;

namespace Application.Services.Interfaces.IRepository.Lesons
{
    public interface ILessonWordRepository
    {
        Task<IEnumerable<LessonWord>> GetAllWordsAsync();

        Task<LessonWord?> GetWordByIdAsync(int id);

        Task<LessonWord> CreateWordsAsync(LessonWord entity);

        Task<LessonWord> UpdateWordsAsync(LessonWord word);

        Task<LessonWord> DeleteWordsAsync(int id);

        Task<bool> ExistsAsync(int id);
    }
}
