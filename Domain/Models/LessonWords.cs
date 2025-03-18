namespace Domain.Models
{
    public class LessonWord
    {
        public int Id { get; set; }
        
        public int LessonId { get; set; }
        
        public string? Name { get; set; }
        
        public string? Translation { get; set; }
        
        public string? ImageUrl { get; set; }
        
        public string? Type { get; set; } // "keyword" or "additional"
        

        public Lesson? Lesson { get; set; }
        public ICollection<UserWordProgress> UserWordProgress { get; set; } = new List<UserWordProgress>();
    }
}
