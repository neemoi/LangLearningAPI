namespace Application.DtoModels.Lessons.Progress
{
    public class UserProgressCreateUpdateDto
    {
        public string? UserId { get; set; }
        
        public int LessonId { get; set; }
        
        public int? QuizId { get; set; }
        
        public decimal Score { get; set; }
        
        public DateTime CompletedAt { get; set; }
        
        public int LearnedWords { get; set; }
    }
}
