using System.ComponentModel.DataAnnotations;

namespace Application.DtoModels.Nouns
{
    public class UpdateNounWordDto
    {
        [Required(ErrorMessage = "Id is required")]
        public int Id { get; set; }

        [StringLength(100, ErrorMessage = "Name cannot be longer than 100 characters")]
        public string? Name { get; set; }

        [Url(ErrorMessage = "ImageUrl must be a valid URL")]
        public string? ImageUrl { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "AlphabetLetterId must be positive")]
        public int? AlphabetLetterId { get; set; }
    }
}
