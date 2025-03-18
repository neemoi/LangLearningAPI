namespace Domain.Models
{
    public class LessonPhrase
    {
        public int Id { get; set; }
        
        public int LessonId { get; set; }
        
        public string? PhraseText { get; set; }
        
        public string? Translation { get; set; }
        
        public string? ImageUrl { get; set; }

        
        public Lesson? Lesson { get; set; }
    }
}
