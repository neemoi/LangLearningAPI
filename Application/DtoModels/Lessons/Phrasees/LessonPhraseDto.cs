namespace Application.DtoModels.Lessons.Phrasees
{
    public class LessonPhraseDto
    {
        public string? Id { get; set; }

        public string? PhraseText { get; set; }
        
        public string? Translation { get; set; }
        
        public string? ImageUrl { get; set; }

        public int LessonId { get; set; }
    }
}
