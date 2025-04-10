using System.ComponentModel.DataAnnotations;

namespace Application.DtoModels.Lessons.Lessons
{
    public class LessonWordDto
    {
        public int Id { get; set; }

        [Required]
        public string? Name { get; set; }

        [Required]
        public string? Translation { get; set; }

        public string? ImageUrl { get; set; }

        public bool IsAdditional { get; set; }
    }
}
