using System.ComponentModel.DataAnnotations;

namespace Application.DtoModels.KidQuiz.Lessons
{
    public class KidLessonDto
    {
        public int Id { get; set; }

        public string Title { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        [Url]
        public string? ImageUrl { get; set; }

        public ICollection<KidQuizQuestionDto> QuizQuestions { get; set; } = new List<KidQuizQuestionDto>();
        public ICollection<KidWordCardDto> WordCards { get; set; } = new List<KidWordCardDto>();
    }
}
