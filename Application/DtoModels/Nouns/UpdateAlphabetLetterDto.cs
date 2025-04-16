using System.ComponentModel.DataAnnotations;

namespace Application.DtoModels.Nouns
{
    public class UpdateAlphabetLetterDto
    {
        [Required]
        public int Id { get; set; }

        [StringLength(1, ErrorMessage = "Symbol must be exactly 1 character")]
        public string? Symbol { get; set; }

        [Url(ErrorMessage = "Invalid URL format")]
        public string? ImageUrl { get; set; }
    }
}
