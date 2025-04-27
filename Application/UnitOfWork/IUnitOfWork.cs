using Application.Services.Implementations.Auth.JWT;
using Application.Services.Interfaces.IRepository.Auth;
using Application.Services.Interfaces.IRepository.Functions;
using Application.Services.Interfaces.IRepository.KidQuiz;
using Application.Services.Interfaces.IRepository.Lesons;
using Application.Services.Interfaces.IRepository.Lessons;
using Application.Services.Interfaces.IRepository.MainQuestions;
using Application.Services.Interfaces.IRepository.Name;
using Application.Services.Interfaces.IRepository.Nouns;
using Application.Services.Interfaces.IRepository.Profile;
using Application.Services.Interfaces.IRepository.Pronunciation;
using Application.Services.Interfaces.IServices.Auth;
using Application.Services.Interfaces.IServices.Lesons;
using Domain.Models;

namespace Application.UnitOfWork
{
    public interface IUnitOfWork
    {
        public IAuthRepository AuthRepository{ get; }

        public IUserRepository UserRepository { get; }

        public IAuthEmailService AuthEmailService { get; }

        public ILessonRepository LessonRepository { get; }

        public ILessonWordRepository LessonWordRepository { get; }

        public ILessonPhraseRepository LessonPhraseRepository { get; }

        public IQuizRepository QuizRepository { get; }

        public IQuizQuestionRepository QuizQuestionRepository { get; }

        public IUserProgressRepository UserProgressRepository { get; }

        public IAlphabetLetterRepository AlphabetLetterRepository { get; }

        public INounWordRepository NounWordRepository { get; }

        public IFunctionWordRepository FunctionWordRepository { get;  }

        public IPartOfSpeechRepository PartOfSpeechRepository { get; }

        public IPronunciationRepository PronunciationRepository { get; }

        public IMainQuestionRepository MainQuestionRepository { get; }

        public IFemaleNameRepository FemaleNameRepository { get; }

        public IMaleNameRepository MaleNameRepository { get; }

        public IEnglishNameRepository EnglishNameRepository { get; }

        public IKidLessonRepository KidLessonRepository{ get; }
        
        public IKidWordCardRepository KidWordCardRepository { get; }

        public IKidQuizQuestionRepository KidQuizQuestionRepository { get; }
        
        public IKidQuizAnswerRepository KidQuizAnswerRepository{ get; }

        public IJwtService JwtService{ get; }

        Task SaveChangesAsync();
    }
}