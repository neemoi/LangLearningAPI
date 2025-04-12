using Application.Services.Implementations.Auth.JWT;
using Application.Services.Interfaces.IRepository.Auth;
using Application.Services.Interfaces.IRepository.Lesons;
using Application.Services.Interfaces.IRepository.Lessons;
using Application.Services.Interfaces.IRepository.Profile;
using Application.Services.Interfaces.IServices.Auth;
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

        public IJwtService JwtService { get; }

        private readonly LanguageLearningDbContext LanguageLearningDbContext;

        public UnitOfWork(LanguageLearningDbContext languageLearningDbContext, IAuthRepository authRepository, IAuthEmailService emailService,
            IJwtService jwtService, IAuthEmailService authEmailService, IUserRepository userRepository, ILessonRepository lessonRepository, 
            ILessonWordRepository lessonWordRepository, ILessonPhraseRepository lessonPhraseRepository, IQuizRepository quizRepository,
            IQuizQuestionRepository quizQuestionRepository)
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
        }

        public async Task SaveChangesAsync()
        {
            await LanguageLearningDbContext.SaveChangesAsync();
        }
    }
}