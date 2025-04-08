using Application.Services.Implementations.Auth.JWT;
using Application.Services.Interfaces.IRepository.Auth;
using Application.Services.Interfaces.IRepository.Lesons;
using Application.Services.Interfaces.IRepository.Profile;
using Application.Services.Interfaces.IServices.Auth;

namespace Application.UnitOfWork
{
    public interface IUnitOfWork
    {
        public IAuthRepository AuthRepository{ get; }

        public IUserRepository UserRepository { get; }

        public IAuthEmailService AuthEmailService { get; }

        public ILessonRepository LessonRepository { get; }

        public ILessonWordRepository LessonWordRepository { get; }

        public IJwtService JwtService{ get; }

        Task SaveChangesAsync();
    }
}