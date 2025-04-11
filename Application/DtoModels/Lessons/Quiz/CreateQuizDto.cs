namespace Application.DtoModels.Lessons.Quiz
{
    public class CreateQuizDto
    {
        public int LessonId { get; set; }
        
        public string? Type { get; set; }
    }
}
