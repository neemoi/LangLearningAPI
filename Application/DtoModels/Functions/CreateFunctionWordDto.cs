namespace Application.DtoModels.Functions
{
    public class CreateFunctionWordDto
    {
        public int Id { get; set; }

        public string? Name { get; set; }
        
        public string? Translation { get; set; }
        
        public int PartOfSpeechId { get; set; }
    }
}
