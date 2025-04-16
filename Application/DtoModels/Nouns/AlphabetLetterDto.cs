namespace Application.DtoModels.Nouns
{
    public class AlphabetLetterDto
    {
        public int Id { get; set; }
        
        public string Symbol { get; set; } = null!;
        
        public string ImageUrl { get; set; } = null!;
        
        public List<NounWordDto> Words { get; set; } = new();
    }
}
