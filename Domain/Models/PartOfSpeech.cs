namespace Domain.Models
{
    public class PartOfSpeech
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!; 
        
        public ICollection<FunctionWord> Words { get; set; } = new List<FunctionWord>();
    }
}
