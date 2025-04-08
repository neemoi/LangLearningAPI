using Application.DtoModels.Auth;
using Domain.Models;
using Microsoft.AspNetCore.Identity;

namespace Application.Services.Interfaces.IRepository.Auth
{
    public interface IAuthRepository
    {
        Task<Users> RegisterUserAsync(AuthRegisterDto registerDto);

        Task<Users> LoginAsync(AuthLoginDto loginDto);

        Task<Users> BlockUserAsync(string userId);

        Task<Users> UnblockUserAsync(string userId);

        Task<IdentityResult> ResetPasswordAsync(ResetPasswordDto resetPasswordDto);

        Task<string> GeneratePasswordResetTokenAsync(string email);

        Task LogoutAsync();
    }
}
