namespace Domain.Models
{
    public class FunctionWord
    {
        public int Id { get; set; }
        
        public string Name { get; set; } = null!; 
        
        public string Translation { get; set; } = null!;
        
        public int PartOfSpeechId { get; set; }
        
        public PartOfSpeech PartOfSpeech { get; set; } = null!;
    }
}
