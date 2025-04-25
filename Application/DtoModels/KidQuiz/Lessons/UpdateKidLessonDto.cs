using System.ComponentModel.DataAnnotations;

namespace Application.DtoModels.KidQuiz.Lessons
{
    public class UpdateKidLessonDto
    {
        public int Id { get; set; }
        
        public string? Title { get; set; } = string.Empty;
        
        public string? Description { get; set; } = string.Empty;
        
        [Url]
        public string? ImageUrl { get; set; }
    }
}
