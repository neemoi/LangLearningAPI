using System.ComponentModel.DataAnnotations;

namespace Application.DtoModels.Nouns
{
    public class CreateAlphabetLetterDto
    {
        public int Id { get; set; }

        public string Symbol { get; set; } = null!;

        public string ImageUrl { get; set; } = null!;
    }
}
