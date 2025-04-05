namespace Application.DtoModels.Lessons.Quiz
{
    public class QuizDto
    {
        public int Id { get; set; }
        
        public string? Type { get; set; }
        
        public string? QuizType { get; set; } 
        
        public List<QuizQuestionDto> Questions { get; set; } = new();
        
        public DateTime CreatedAt { get; set; }
        
        public int LessonId { get; set; }
    }
}
