namespace Domain.Models
{
    public class PronunciationCategory
    {
        public int Id { get; set; }
        
        public string Name { get; set; } = null!;
        
        public ICollection<WordItem> WordItems { get; set; } = new List<WordItem>();
    }
}
