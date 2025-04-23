namespace Application.DtoModels.MainQuestions
{
    public class CreateMainQuestionWordDto
    {
        public string Name { get; set; } = null!;
        
        public string ImagePath { get; set; } = null!;
        
        public int MainQuestionId { get; set; }
    }
}
