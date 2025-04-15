namespace Application.DtoModels.Lessons.Progress
{
    public class UserProgressResponseDto : UserProgressCreateUpdateDto
    {
        public int Id { get; set; }
        
        public string? LessonTitle { get; set; }
        
        public string? QuizType { get; set; }
    }
}
