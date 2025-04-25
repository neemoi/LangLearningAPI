namespace Domain.Models
{
    public class KidQuizQuestion
    {
        public int Id { get; set; }

        public int LessonId { get; set; }
       
        public int WordCardId { get; set; }
        
        public int QuizTypeId { get; set; }

        public string? QuestionText { get; set; }
        
        public string? AudioUrl { get; set; }
        
        public string? ImageUrl { get; set; }
        
        public string? CorrectAnswer { get; set; }

        public KidLesson Lesson { get; set; } = null!;
        public KidWordCard WordCard { get; set; } = null!;
        public KidQuizType QuizType { get; set; } = null!;
        public ICollection<KidQuizAnswer> Answers { get; set; } = new List<KidQuizAnswer>();
    }

}
