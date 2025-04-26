using System.ComponentModel.DataAnnotations;

namespace Application.DtoModels.KidQuiz.KidQuizQuestion
{
    public class CreateKidQuizQuestionDto
    {
        [Required]
        public int LessonId { get; set; }
        
        [Required]
        public int QuizTypeId { get; set; }

        [Required]
        public int WordCardId { get; set; }

        public string? QuestionText { get; set; }
        
        [Url]
        public string? AudioUrl { get; set; }
        
        [Url]
        public string? ImageUrl { get; set; }
        
        public string? CorrectAnswer { get; set; }
    }
}
