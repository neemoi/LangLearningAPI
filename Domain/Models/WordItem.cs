namespace Domain.Models
{
    public class WordItem
    {
        public int Id { get; set; }
        
        public string Name { get; set; } = null!;
        
        public string ImagePath { get; set; } = null!;
        
        public int CategoryId { get; set; }

        public PronunciationCategory Category { get; set; } = null!;
    }
}
