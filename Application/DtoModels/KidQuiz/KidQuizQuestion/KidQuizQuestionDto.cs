using System.ComponentModel.DataAnnotations;

namespace Application.DtoModels.KidQuiz
{
    public class KidQuizQuestionDto
    {
        public int Id { get; set; }
        
        public int LessonId { get; set; }
        
        public int QuizTypeId { get; set; }
        
        public string? QuestionText { get; set; }
        
        [Url]
        public string? AudioUrl { get; set; }
        
        [Url]
        public string? ImageUrl { get; set; }
        
        public string? CorrectAnswer { get; set; }
        
        public int WordCardId { get; set; }
    }
}