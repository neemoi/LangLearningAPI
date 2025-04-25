namespace Domain.Models
{
    public class KidQuizAnswer
    {
        public int Id { get; set; }

        public int QuestionId { get; set; }
        
        public string AnswerText { get; set; } = string.Empty;
        
        public bool IsCorrect { get; set; } = false;

        public KidQuizQuestion Question { get; set; } = null!;
    }
}
