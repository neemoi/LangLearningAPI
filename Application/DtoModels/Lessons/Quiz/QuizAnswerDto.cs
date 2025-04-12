namespace Application.DtoModels.Lessons.Quiz
{
    public class QuizAnswerDto
    {
        public int Id { get; set; }
        public string? AnswerText { get; set; }
        public bool IsCorrect { get; set; }
    }

}
