using Application.DtoModels.Lessons.Phrasees;
using Application.DtoModels.Lessons.Quiz;

namespace Application.DtoModels.Lessons.Lessons
{
    public class LessonDto
    {
        public int Id { get; set; }

        public string? Title { get; set; }

        public string? Description { get; set; }

        public string? VideoUrl { get; set; }

        public string? PdfUrl { get; set; }

        public DateTime CreatedAt { get; set; }

        public List<LessonWordDto> Words { get; set; } = new();
        public List<LessonPhraseDto> Phrases { get; set; } = new();
        public List<QuizDto> Quizzes { get; set; } = new();
    }

}
