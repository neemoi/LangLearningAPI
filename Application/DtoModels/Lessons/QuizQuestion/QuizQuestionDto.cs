using Application.DtoModels.Lessons.Quiz;

namespace Application.DtoModels.Lessons.QuizQuestion
{
    public class QuizQuestionDto
    {
        public int Id { get; set; }

        public string? QuestionType { get; set; }

        public string? QuestionText { get; set; }

        public string? ImageUrl { get; set; }

        public string? AudioUrl { get; set; }

        public List<QuizAnswerDto> Answers { get; set; } = new();

        public string? CorrectAnswer { get; set; }

        public int? TimeLimit { get; set; }

        public bool HasMedia { get; set; }
    }
}