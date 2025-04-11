namespace Application.DtoModels.Lessons.Quiz
{
    public class UpdateQuizDto
    {
        public int Id { get; set; }
        
        public int? LessonId { get; set; }
        
        public string? Type { get; set; }
    }
}
