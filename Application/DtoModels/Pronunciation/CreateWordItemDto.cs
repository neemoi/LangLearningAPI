namespace Application.DtoModels.Pronunciation
{
    public class CreateWordItemDto
    {
        public string Name { get; set; } = null!;
        
        public string ImagePath { get; set; } = null!;

        public int CategoryId { get; set; }
    }
}
