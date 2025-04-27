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
using Application.UnitOfWork;
using Infrastructure.Data;

namespace Persistance.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        public IAuthRepository AuthRepository { get; }

        public IUserRepository UserRepository{ get; }

        public IAuthEmailService AuthEmailService { get; }

        public ILessonRepository LessonRepository { get; }

        public ILessonWordRepository LessonWordRepository { get; }

        public ILessonPhraseRepository LessonPhraseRepository { get; }

        public IQuizRepository QuizRepository { get; }

        public IQuizQuestionRepository QuizQuestionRepository { get; }

        public IUserProgressRepository UserProgressRepository { get;  }

        public IAlphabetLetterRepository AlphabetLetterRepository { get; }

        public INounWordRepository NounWordRepository { get; }

        public IFunctionWordRepository FunctionWordRepository { get; }

        public IPartOfSpeechRepository PartOfSpeechRepository { get; }

        public IPronunciationRepository PronunciationRepository { get; }

        public IMainQuestionRepository MainQuestionRepository { get; }

        public IFemaleNameRepository FemaleNameRepository { get; }

        public IMaleNameRepository MaleNameRepository { get; }

        public IEnglishNameRepository EnglishNameRepository { get; }

        public IKidWordCardRepository KidWordCardRepository { get; }

        public IKidLessonRepository KidLessonRepository {  get; }

        public IKidQuizQuestionRepository KidQuizQuestionRepository { get; }

        public IKidQuizAnswerRepository KidQuizAnswerRepository { get; }

        public IJwtService JwtService { get; }

        private readonly LanguageLearningDbContext LanguageLearningDbContext;

        public UnitOfWork(LanguageLearningDbContext languageLearningDbContext, IAuthRepository authRepository, IAuthEmailService emailService,
            IJwtService jwtService, IAuthEmailService authEmailService, IUserRepository userRepository, ILessonRepository lessonRepository,
            ILessonWordRepository lessonWordRepository, ILessonPhraseRepository lessonPhraseRepository, IQuizRepository quizRepository,
            IQuizQuestionRepository quizQuestionRepository, IUserProgressRepository userProgressRepository, IAlphabetLetterRepository alphabetLetterRepository,
            INounWordRepository nounWordRepository, IFunctionWordRepository functionWordRepository, IPartOfSpeechRepository partOfSpeechRepository,
            IPronunciationRepository pronunciationRepository, IMainQuestionRepository mainQuestionRepository, IFemaleNameRepository femaleNameRepository,
            IMaleNameRepository maleNameRepository, IEnglishNameRepository englishNameRepository, IKidLessonRepository kidLessonRepository,
            IKidWordCardRepository kidWordCardRepository, IKidQuizQuestionRepository kidQuizQuestionRepository, IKidQuizQuestionRepository kidQuizQuestionRepository1,
            IKidQuizAnswerRepository kidQuizAnswerRepository)
        {
            LanguageLearningDbContext = languageLearningDbContext;
            AuthRepository = authRepository;
            AuthEmailService = authEmailService;
            JwtService = jwtService;
            UserRepository = userRepository;
            LessonRepository = lessonRepository;
            LessonWordRepository = lessonWordRepository;
            LessonPhraseRepository = lessonPhraseRepository;
            QuizRepository = quizRepository;
            QuizQuestionRepository = quizQuestionRepository;
            UserProgressRepository = userProgressRepository;
            AlphabetLetterRepository = alphabetLetterRepository;
            NounWordRepository = nounWordRepository;
            FunctionWordRepository = functionWordRepository;
            PartOfSpeechRepository = partOfSpeechRepository;
            PronunciationRepository = pronunciationRepository;
            MainQuestionRepository = mainQuestionRepository;
            FemaleNameRepository = femaleNameRepository;
            MaleNameRepository = maleNameRepository;
            EnglishNameRepository = englishNameRepository;
            KidLessonRepository = kidLessonRepository;
            KidWordCardRepository = kidWordCardRepository;
            KidQuizQuestionRepository = kidQuizQuestionRepository;
            KidQuizAnswerRepository = kidQuizAnswerRepository;
        }

        public async Task SaveChangesAsync()
        {
            await LanguageLearningDbContext.SaveChangesAsync();
        }
    }
}