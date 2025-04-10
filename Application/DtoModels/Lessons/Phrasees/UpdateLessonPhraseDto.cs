using System.ComponentModel.DataAnnotations;

namespace Application.DtoModels.Lessons.Phrasees
{
    public class UpdateLessonPhraseDto
    {
        public int LessonId { get; set; }
            
        public int PhraseId { get; set; }

        public string? PhraseText { get; set; }

        public string? Translation { get; set; }

        public string? ImageUrl { get; set; }
    }
}
