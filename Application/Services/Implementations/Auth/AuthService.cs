using Application.DtoModels.Auth;
using Application.DtoModels.Auth.Response;
using Application.Services.Interfaces.IServices;
using AutoMapper;
using Domain.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Application.UnitOfWork;

namespace Application.Services.Implementations.Auth
{
    public class AuthService : IAuthService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger<AuthService> _logger;
        private readonly UserManager<Users> _userManager;
        private readonly IConfiguration _configuration;

        public AuthService(IUnitOfWork unitOfWork, IMapper mapper, ILogger<AuthService> logger, UserManager<Users> userManager,
            IConfiguration configuration)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logger = logger;
            _userManager = userManager;
            _configuration = configuration;
        }

        public async Task<AuthRegisterResponseDto> RegisterUserAsync(AuthRegisterDto registerDto)
        {
            if (registerDto == null)
            {
                throw new ArgumentNullException(nameof(registerDto), "RegisterDto cannot be null");
            }

            try
            {
                _logger.LogInformation("Starting user registration: {Email}", registerDto.Email);

                var user = await _unitOfWork.AuthRepository.RegisterUserAsync(registerDto);
                var userRoles = await _userManager.GetRolesAsync(user);
                var token = await _unitOfWork.JwtService.GenerateTokenAsync(user);

                var response = _mapper.Map<AuthRegisterResponseDto>(user);
                response.Role = userRoles.FirstOrDefault();
                response.Token = token;

                _logger.LogInformation("User registered successfully: {Email}", registerDto.Email);

                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred during user registration: {Email}", registerDto.Email);
                throw;
            }
        }

        public async Task<AuthLoginResponseDto> LoginAsync(AuthLoginDto loginDto)
        {
            if (loginDto == null)
            {
                throw new ArgumentNullException(nameof(loginDto), "LoginDto cannot be null");
            }

            try
            {
                _logger.LogInformation("User attempting login: {EmailOrUsername}", loginDto.EmailOrUserName);

                var user = await _unitOfWork.AuthRepository.LoginAsync(loginDto);
                var userRoles = await _userManager.GetRolesAsync(user);
                var token = await _unitOfWork.JwtService.GenerateTokenAsync(user);

                var response = _mapper.Map<AuthLoginResponseDto>(user);
                response.Role = userRoles.FirstOrDefault();
                response.Token = token;

                _logger.LogInformation("User logged in successfully: {EmailOrUsername}", loginDto.EmailOrUserName);

                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred during user login: {EmailOrUsername}", loginDto.EmailOrUserName);
                throw;
            }
        }

        public async Task<UserStatusResponseDto> BlockUserAsync(string userId)
        {
            if (string.IsNullOrEmpty(userId))
            {
                throw new ArgumentNullException(nameof(userId), "UserId cannot be null or empty");
            }

            try
            {
                _logger.LogInformation("Attempting to block user: {UserId}", userId);

                var user = await _unitOfWork.AuthRepository.BlockUserAsync(userId);
                var response = _mapper.Map<UserStatusResponseDto>(user);

                _logger.LogInformation("User {UserId} has been successfully blocked", userId);

                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while blocking user: {UserId}", userId);
                throw;
            }
        }

        public async Task<UserStatusResponseDto> UnblockUserAsync(string userId)
        {
            if (string.IsNullOrEmpty(userId))
            {
                throw new ArgumentNullException(nameof(userId), "UserId cannot be null or empty");
            }

            try
            {
                _logger.LogInformation("Attempting to unblock user: {UserId}", userId);

                var user = await _unitOfWork.AuthRepository.UnblockUserAsync(userId);
                var response = _mapper.Map<UserStatusResponseDto>(user);

                _logger.LogInformation("User {UserId} has been successfully unblocked", userId);

                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while unblocking user: {UserId}", userId);
                throw;
            }
        }

        public async Task<string> GeneratePasswordResetTokenAsync(ForgotPasswordDto forgotPasswordDto)
        {
            if (forgotPasswordDto == null || string.IsNullOrEmpty(forgotPasswordDto.Email))
            {
                throw new ArgumentNullException(nameof(forgotPasswordDto.Email), "Email cannot be null or empty");
            }

            try
            {
                _logger.LogInformation("Generating password reset token for email: {Email}", forgotPasswordDto.Email);

                var token = await _unitOfWork.AuthRepository.GeneratePasswordResetTokenAsync(forgotPasswordDto.Email);

                var resetLink = $"{_configuration["App:BaseUrl"]}/api/auth/reset-password?email={forgotPasswordDto.Email}&token={Uri.EscapeDataString(token)}";

                await SendPasswordResetEmailAsync(forgotPasswordDto.Email, resetLink);

                _logger.LogInformation("Password reset token generated successfully for email: {Email}", forgotPasswordDto.Email);

                return token;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error generating password reset token for email: {Email}", forgotPasswordDto.Email);
                throw;
            }
        }

        public async Task<IdentityResult> ResetPasswordAsync(ResetPasswordDto resetPasswordDto)
        {
            if (resetPasswordDto == null || string.IsNullOrEmpty(resetPasswordDto.Email) || string.IsNullOrEmpty(resetPasswordDto.Token) || string.IsNullOrEmpty(resetPasswordDto.NewPassword))
            {
                throw new ArgumentNullException(nameof(resetPasswordDto), "ResetPasswordDto properties cannot be null or empty");
            }

            try
            {
                _logger.LogInformation("Attempting to reset password for email: {Email}", resetPasswordDto.Email);

                var result = await _unitOfWork.AuthRepository.ResetPasswordAsync(resetPasswordDto);

                if (result.Succeeded)
                {
                    _logger.LogInformation("Password reset successful for email: {Email}", resetPasswordDto.Email);
                }
                else
                {
                    _logger.LogWarning("Password reset failed for email {Email}: {Errors}", resetPasswordDto.Email, string.Join(", ", result.Errors.Select(e => e.Description)));
                }

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred during password reset for email: {Email}", resetPasswordDto.Email);
                throw;
            }
        }

        public async Task LogoutAsync()
        {
            try
            {
                await _unitOfWork.AuthRepository.LogoutAsync();

                _logger.LogInformation("Logout successful");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred during logout");
                throw;
            }
        }

        private async Task SendPasswordResetEmailAsync(string email, string resetLink)
        {
            if (string.IsNullOrEmpty(email))
            {
                throw new ArgumentNullException(nameof(email), "Email cannot be null or empty");
            }

            var subject = "Восстановление пароля";
            var body = $"<p>Для восстановления пароля, пожалуйста, перейдите по следующей ссылке:</p><a href='{resetLink}'>Восстановить пароль</a>";

            await _unitOfWork.AuthEmailService.SendEmailAsync(email, subject, body);
        }
    }
}