using System.ComponentModel.DataAnnotations;

namespace Application.DtoModels.KidQuiz.KidWordCard
{
    public class CreateKidWordCardDto
    {
        public int KidLessonId { get; set; }
        
        public string? Word { get; set; }
        
        [Url]
        public string? ImageUrl { get; set; }

        [Url]
        public string? AudioUrl { get; set; }
    }
}
