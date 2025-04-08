using System.ComponentModel.DataAnnotations;

namespace Application.DtoModels.Lessons.Words
{
    public class CreateLessonWordDto
    {
        [Required]
        public int LessonId { get; set; }

        [Required]
        public string Name { get; set; } = string.Empty;

        [Required]
        public string Translation { get; set; } = string.Empty;

        [Url]
        public string? ImageUrl { get; set; }

        [Required]
        [RegularExpression("keyword|additional")]
        public string Type { get; set; } = "keyword";
    }
}
