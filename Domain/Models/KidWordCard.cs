namespace Domain.Models
{
    public class KidWordCard
    {
        public int Id { get; set; }
        
        public int LessonId { get; set; }

        public string Word { get; set; } = string.Empty;
        
        public string ImageUrl { get; set; } = string.Empty;
        
        public string AudioUrl { get; set; } = string.Empty;

        public KidLesson Lesson { get; set; } = null!;
    }

}
