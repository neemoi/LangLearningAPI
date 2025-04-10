using Application.DtoModels.Lessons.Phrasees;
using Application.DtoModels.Lessons.Quiz;

namespace Application.DtoModels.Lessons.Lessons
{
    public class LessonDetailDto : LessonDto
    {
        public int TotalWordsCount { get; set; }

        public int CompletedWordsCount { get; set; }

        public double ProgressPercentage =>
            TotalWordsCount > 0 ? CompletedWordsCount * 100.0 / TotalWordsCount : 0;

        public List<LessonWordDto> Words { get; set; } = new();
        public List<LessonPhraseDto> Phrases { get; set; } = new();
        public List<QuizDto> Quizzes { get; set; } = new();
    }
}
