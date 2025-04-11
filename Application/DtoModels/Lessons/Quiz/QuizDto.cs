namespace Application.DtoModels.Lessons.Quiz
{
    public class QuizDto
    {
        public int Id { get; set; }
        
        public int LessonId { get; set; }

        public string? Type { get; set; } // "nouns" or "grammar"
        
        public DateTime CreatedAt { get; set; }
    }
}
