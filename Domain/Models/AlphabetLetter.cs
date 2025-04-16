namespace Domain.Models
{
    public class AlphabetLetter
    {
        public int Id { get; set; }
        public string Symbol { get; set; } = null!;
        public string ImageUrl { get; set; } = null!;

        public ICollection<NounWord> Words { get; set; } = new List<NounWord>();
    }

}
