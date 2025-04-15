namespace Application.DtoModels.Lessons.Progress
{
    public class UserWordProgressResponseDto
    {
        public int Id { get; set; }
        
        public string? UserId { get; set; }
        
        public int LessonId { get; set; }
        
        public int WordId { get; set; }
        
        public string? QuestionType { get; set; }
        
        public bool IsCorrect { get; set; }
        
        public string? WordText { get; set; }
    }
}
