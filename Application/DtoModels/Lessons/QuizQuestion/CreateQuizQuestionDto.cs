using Application.DtoModels.Lessons.Quiz;

namespace Application.DtoModels.Lessons.QuizQuestion
{
    public class CreateQuizQuestionDto
    {
        public int QuizId { get; set; }
        
        public string? QuestionType { get; set; }
        
        public string? QuestionText { get; set; }
        
        public string? ImageUrl { get; set; }
        
        public string? AudioUrl { get; set; }
        
        public string? CorrectAnswer { get; set; }
        
        public List<QuizAnswerDto> Answers { get; set; } = new();
    }
}
