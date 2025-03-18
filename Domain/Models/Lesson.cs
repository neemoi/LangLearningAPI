namespace Domain.Models
{
    public class Lesson
    {
        public int Id { get; set; }
        
        public string? Title { get; set; }
        
        public string? Description { get; set; }
        
        public string? VideoUrl { get; set; }
        
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;


        public ICollection<LessonWord> Words { get; set; } = new List<LessonWord>();
        public ICollection<LessonPhrase> Phrases { get; set; } = new List<LessonPhrase>();
        public ICollection<Quiz> Quizzes { get; set; } = new List<Quiz>();
    }
}
