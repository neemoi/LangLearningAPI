using System.ComponentModel.DataAnnotations;

namespace Application.DtoModels.KidQuiz.Lessons
{
    public class CreateKidLessonDto
    {
        public string Title { get; set; } = string.Empty;
       
        public string Description { get; set; } = string.Empty;

        [Url]
        public string? ImageUrl { get; set; }
    }
}
