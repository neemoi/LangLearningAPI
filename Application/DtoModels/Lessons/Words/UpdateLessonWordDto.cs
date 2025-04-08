using System.ComponentModel.DataAnnotations;

namespace Application.DtoModels.Lessons.Words
{
    public class UpdateLessonWordDto
    {
        public string? Name { get; set; }

        public string? Translation { get; set; }

        [Url]
        public string? ImageUrl { get; set; }

        [RegularExpression("keyword|additional", ErrorMessage = "Type must be either 'keyword' or 'additional'.")]
        public string? Type { get; set; }
    }
}
