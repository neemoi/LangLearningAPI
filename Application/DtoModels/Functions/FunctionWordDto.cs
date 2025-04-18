namespace Application.DtoModels.Functions
{
    public class FunctionWordDto
    {
        public int Id { get; set; }
        
        public string Name { get; set; } = null!;
        
        public string Translation { get; set; } = null!;
        
        public int PartOfSpeechId { get; set; }
    }
}
