namespace Domain.Models
{
    public class UserProgress
    {
        public int Id { get; set; }
        
        public string? UserId { get; set; }
        
        public int LessonId { get; set; }
        
        public int? QuizId { get; set; }
        
        public decimal Score { get; set; } = 0;
        
        public DateTime CompletedAt { get; set; } = DateTime.UtcNow;
        
        public int LearnedWords { get; set; } = 0;


        public Users? User { get; set; }
        public Lesson? Lesson { get; set; }
        public Quiz? Quiz { get; set; }
    }
}