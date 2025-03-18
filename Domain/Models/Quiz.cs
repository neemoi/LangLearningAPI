namespace Domain.Models
{
    public class Quiz
    {
        public int Id { get; set; }
        
        public int LessonId { get; set; }
        
        public string? Type { get; set; } // "nouns" or "grammar"
        
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        
        public Lesson? Lesson { get; set; }
        public ICollection<QuizQuestion> Questions { get; set; } = new List<QuizQuestion>();
    }
}
