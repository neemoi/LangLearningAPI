using Application.DtoModels.Auth;
using Application.DtoModels.Auth.Response;
using Microsoft.AspNetCore.Identity;

namespace Application.Services.Interfaces.IServices.Auth
{
    public interface IAuthService
    {
        Task<AuthRegisterResponseDto> RegisterUserAsync(AuthRegisterDto registerDto);

        Task<AuthLoginResponseDto> LoginAsync(AuthLoginDto loginDto);

        Task<UserStatusResponseDto> BlockUserAsync(string userId);

        Task<UserStatusResponseDto> UnblockUserAsync(string userId);

        Task<IdentityResult> ResetPasswordAsync(ResetPasswordDto resetPasswordDto);

        Task<string> GeneratePasswordResetTokenAsync(ForgotPasswordDto forgotPasswordDto);

        Task LogoutAsync();
    }
}
