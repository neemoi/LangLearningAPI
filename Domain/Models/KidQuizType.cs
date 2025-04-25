namespace Domain.Models
{
    public class KidQuizType
    {
        public int Id { get; set; }
        
        public string Name { get; set; } = string.Empty; //"image_choice", "audio_choice", ...

        public ICollection<KidQuizQuestion> Questions { get; set; } = new List<KidQuizQuestion>();
    }
}
