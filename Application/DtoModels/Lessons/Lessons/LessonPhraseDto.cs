using System.ComponentModel.DataAnnotations;

namespace Application.DtoModels.Lessons.Lessons
{
    public class LessonPhraseDto
    {
        public int Id { get; set; }
        
        [Required]
        public string? PhraseText { get; set; }
        
        [Required]
        public string? Translation { get; set; }
        
        public string? ImageUrl { get; set; }
      
        public bool IsComplex { get; set; } 
    }
}
