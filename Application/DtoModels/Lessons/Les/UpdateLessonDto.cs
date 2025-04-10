using System.ComponentModel.DataAnnotations;

namespace Application.DtoModels.Lessons.Lessons
{
    public class UpdateLessonDto
    {
        [Required]
        public int Id { get; set; }

        public string? Title { get; set; }

        public string? Description { get; set; }
        
        [Url]
        public string? VideoUrl { get; set; }
        
        [Url]
        public string? PdfUrl { get; set; }
    }
}
