namespace Application.DtoModels.Lessons.Progress
{
    public class UserAnswerDto
    {
        public string? UserId { get; set; }
        
        public int LessonId { get; set; }
        
        public int QuestionId { get; set; }
        
        public int AnswerId { get; set; } 
        
        public bool IsCorrect { get; set; }  
    }

}
