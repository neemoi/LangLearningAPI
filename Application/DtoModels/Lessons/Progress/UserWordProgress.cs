
using Domain.Models;

namespace Application.DtoModels.Lessons.Progress
{
    public class UserWordProgress
    {
        public int Id { get; set; }
        
        public string? UserId { get; set; }
        
        public int LessonId { get; set; }
        
        public int WordId { get; set; }

        public QuestionType QuestionType { get; set; }

        public bool IsCorrect { get; set; } = false;
    }

}
