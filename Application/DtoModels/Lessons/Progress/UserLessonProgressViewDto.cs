namespace Application.DtoModels.Lessons.Progress
{
    public class UserLessonProgressViewDto
    {
        public string? UserId { get; set; }
        public string? LessonTitle { get; set; }
        public string? LessonDescription { get; set; }
        public string? VideoUrl { get; set; }
        public string? PdfUrl { get; set; }

        public string QuizType { get; set; }
        public int TotalQuestions { get; set; }
        public int CorrectAnswers { get; set; }

        public int LearnedWords { get; set; }
        public decimal Score { get; set; }
        public DateTime CompletedAt { get; set; }

        public List<WordProgressDto>? Words { get; set; }
    }
}
