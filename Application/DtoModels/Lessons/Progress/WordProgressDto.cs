namespace Application.DtoModels.Lessons.Progress
{
    public class WordProgressDto
    {
        public int WordId { get; set; }
        
        public string? WordText { get; set; }
        
        public bool IsCorrect { get; set; }
        
        public string? QuestionType { get; set; }
    }
}
