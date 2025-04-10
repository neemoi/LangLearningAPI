using System.ComponentModel.DataAnnotations;

namespace Application.DtoModels.Lessons.Lessons
{
    public class CreateLessonDto
    {
        [Required]
        public string? Title { get; set; }

        [Required]
        public string? Description { get; set; }
        
        [Url]
        public string? VideoUrl { get; set; }

        [Url]
        public string? PdfUrl { get; set; }
    }
}
