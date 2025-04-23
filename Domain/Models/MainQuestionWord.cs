namespace Domain.Models
{
    public class MainQuestionWord
    {
        public int Id { get; set; }
        
        public string Name { get; set; } = null!;
        
        public string ImagePath { get; set; } = null!;

        public int MainQuestionId { get; set; }
        public MainQuestion MainQuestion { get; set; } = null!;
    }
}
