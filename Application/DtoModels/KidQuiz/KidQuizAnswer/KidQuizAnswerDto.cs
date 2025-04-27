namespace Application.DtoModels.KidQuiz.KidQuizAnswer
{
    public class KidQuizAnswerDto
    {
        public int Id { get; set; }
        
        public int QuestionId { get; set; }
        
        public string AnswerText { get; set; } = string.Empty;
        
        public bool IsCorrect { get; set; }
    }
}
