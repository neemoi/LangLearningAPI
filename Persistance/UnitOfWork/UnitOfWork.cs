using Application.Services.Implementations.Auth.JWT;
using Application.Services.Interfaces.IRepository;
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

        public IJwtService JwtService { get; }


        private readonly LanguageLearningDbContext LanguageLearningDbContext;

        public UnitOfWork(LanguageLearningDbContext languageLearningDbContext, IAuthRepository authRepository, IAuthEmailService emailService,
            IJwtService jwtService, IAuthEmailService authEmailService, IUserRepository userRepository)
        {
            LanguageLearningDbContext = languageLearningDbContext;
            AuthRepository = authRepository;
            AuthEmailService = authEmailService;
            JwtService = jwtService;
            UserRepository = userRepository;
        }

        public async Task SaveChangesAsync()
        {
            await LanguageLearningDbContext.SaveChangesAsync();
        }
    }
}