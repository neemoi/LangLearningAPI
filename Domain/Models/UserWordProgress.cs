namespace Domain.Models
{
    public class UserWordProgress
    {
        public int Id { get; set; }
        
        public string? UserId { get; set; }
        
        public int LessonId { get; set; }
        
        public int WordId { get; set; }
        
        public string? QuestionType { get; set; } //Question type ('image_choice', 'audio_choice',
                                                  //'image_audio_choice', 'spelling', 'grammar selection',
                                                  //'pronunciation', 'advanced survey' )

        public bool IsCorrect { get; set; } = false;


        public Users? User { get; set; }
        public Lesson? Lesson { get; set; }
        public LessonWord? Word { get; set; }
    }
}
