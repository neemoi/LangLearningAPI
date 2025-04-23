namespace Application.DtoModels.MainQuestions
{
    public class MainQuestionDto
    {
        public int Id { get; set; }
        
        public string Name { get; set; } = null!;
        
        public string ImagePath { get; set; } = null!;
        
        public List<MainQuestionWordDto> Words { get; set; } = new();
    }
}