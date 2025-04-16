namespace Application.DtoModels.Nouns
{
    public class NounWordDto
    {
        public int Id { get; set; }
        
        public string? Name { get; set; } = null!;
        
        public string? ImageUrl { get; set; } = null!;
        
        public int AlphabetLetterId { get; set; }
    }
}