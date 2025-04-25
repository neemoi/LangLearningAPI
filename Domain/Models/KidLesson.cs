namespace Domain.Models
{
    public class KidLesson
    {
        public int Id { get; set; }
        
        public string Title { get; set; } = string.Empty;
        
        public string Description { get; set; } = string.Empty;

        public string? ImageUrl { get; set; }

        public ICollection<KidQuizQuestion> QuizQuestions { get; set; } = new List<KidQuizQuestion>();
        public ICollection<KidWordCard> WordCards { get; set; } = new List<KidWordCard>();
    }
}
