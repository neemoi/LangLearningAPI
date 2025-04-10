using System.ComponentModel.DataAnnotations;

namespace Application.DtoModels.Lessons.Phrasees
{
    public class CreateLessonPhraseDto
    {
        [Required]
        public int LessonId { get; set; }
       
        [Required]
        public string? PhraseText { get; set; }

        [Required]
        public string? Translation { get; set; }

        public string? ImageUrl { get; set; }

    }
}
