namespace Application.DtoModels.KidQuiz.KidQuizAnswer
{
    public class CreateKidQuizAnswerDto
    {
        public int QuestionId { get; set; }
       
        public string AnswerText { get; set; } = string.Empty;
        
        public bool IsCorrect { get; set; } = false;
    }
}
