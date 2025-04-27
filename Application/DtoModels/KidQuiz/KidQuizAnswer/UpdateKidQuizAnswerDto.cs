namespace Application.DtoModels.KidQuiz.KidQuizAnswer
{
    public class UpdateKidQuizAnswerDto
    {
        public int Id { get; set; }
        
        public int? QuestionId { get; set; }
        
        public string? AnswerText { get; set; }
        
        public bool? IsCorrect { get; set; }
    }
}
