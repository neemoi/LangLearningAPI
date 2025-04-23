namespace Domain.Models
{
    public class MainQuestion
    {
        public int Id { get; set; }

        public string Name { get; set; } = null!;

        public string ImagePath { get; set; } = null!;

        public ICollection<MainQuestionWord> Words { get; set; } = new List<MainQuestionWord>();
    }
}
