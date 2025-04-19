namespace Application.DtoModels.Pronunciation
{
    public class PronunciationCategoryDto
    {
        public int Id { get; set; }
        
        public string Name { get; set; } = null!;
        
        public List<WordItemDto> WordItems { get; set; } = new();
    }
}
