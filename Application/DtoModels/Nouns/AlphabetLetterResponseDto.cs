namespace Application.DtoModels.Nouns
{
    public class AlphabetLetterResponseDto
    {
        public int Id { get; set; }
        
        public string Symbol { get; set; } = null!;
        
        public string ImageUrl { get; set; } = null!;
    }
}
