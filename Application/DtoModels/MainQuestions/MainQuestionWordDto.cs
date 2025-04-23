namespace Application.DtoModels.MainQuestions
{
    public class MainQuestionWordDto
    {
        public int Id { get; set; }
        
        public string Name { get; set; } = null!;
        
        public string ImagePath { get; set; } = null!;
        
        public int MainQuestionId { get; set; }
    }
}