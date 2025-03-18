namespace Domain.Models
{
    public class QuizQuestion
    {
        public int Id { get; set; }
        
        public int QuizId { get; set; }
        
        public string? QuestionType { get; set; } //Question type ('image_choice', 'audio_choice',
                                                  //'image_audio_choice', 'spelling', 'grammar selection',
                                                  //'pronunciation', 'advanced survey' )

        public string? QuestionText { get; set; }
        
        public string? ImageUrl { get; set; }
        
        public string? AudioUrl { get; set; }
        
        public string? CorrectAnswer { get; set; }


        public Quiz? Quiz { get; set; }
        public ICollection<QuizAnswer> Answers { get; set; } = new List<QuizAnswer>();
    }
}
