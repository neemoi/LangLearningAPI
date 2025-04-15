using System.ComponentModel.DataAnnotations;

namespace Application.DtoModels.Lessons.Progress
{
    public class UserWordProgressCreateDto
    {
        public string? UserId { get; set; }
        
        public int LessonId { get; set; }
        
        public int WordId { get; set; }

        [Required]
        [EnumDataType(typeof(QuestionType))]
        public QuestionType QuestionType { get; set; }

        public bool IsCorrect { get; set; }

        public int LessonWordId { get; set; }
    }
}
