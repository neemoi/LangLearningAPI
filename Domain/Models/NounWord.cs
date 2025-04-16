namespace Domain.Models
{
    public class NounWord
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string ImageUrl { get; set; } = null!;

        public int AlphabetLetterId { get; set; }
        public AlphabetLetter AlphabetLetter { get; set; } = null!;
    }

}
