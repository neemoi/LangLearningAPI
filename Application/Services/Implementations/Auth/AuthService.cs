using Application.DtoModels.Auth;
using Application.DtoModels.Auth.Response;
using Application.Services.Interfaces.IServices.Auth;
using Application.UnitOfWork;
using AutoMapper;
using Domain.Models;
using LangLearningAPI.Exceptions;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.ComponentModel.DataAnnotations;
using System.Net;

namespace Application.Services.Implementations.Auth
{
    public class AuthService : IAuthService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger<AuthService> _logger;
        private readonly UserManager<Users> _userManager;
        private readonly IConfiguration _configuration;

        public AuthService(IUnitOfWork unitOfWork, IMapper mapper, ILogger<AuthService> logger,
            UserManager<Users> userManager, IConfiguration configuration)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logger = logger;
            _userManager = userManager;
            _configuration = configuration;
        }

        public async Task<AuthRegisterResponseDto> RegisterUserAsync(AuthRegisterDto registerDto)
        {
            try
            {
                ValidateAuthDto(registerDto);
                _logger.LogInformation("Starting user registration: {Email}", registerDto.Email);

                var existingUser = await _userManager.FindByEmailAsync(registerDto.Email);
                if (existingUser != null)
                {
                    _logger.LogWarning("Email already exists: {Email}", registerDto.Email);
                    throw new IdentityException("Email already exists", "USER_EMAIL_EXISTS");
                }

                var user = await _unitOfWork.AuthRepository.RegisterUserAsync(registerDto);
                var userRoles = await _userManager.GetRolesAsync(user);
                var token = await _unitOfWork.JwtService.GenerateTokenAsync(user);

                var response = _mapper.Map<AuthRegisterResponseDto>(user);
                response.Role = userRoles.FirstOrDefault();
                response.Token = token;

                _logger.LogInformation("User registered successfully: {Email}", registerDto.Email);
                
                return response;
            }
            catch (Exception ex) when (ex is not ApiException)
            {
                _logger.LogError(ex, "Error during user registration: {Email}", registerDto.Email);
                throw new ApiException(
                    (int)HttpStatusCode.InternalServerError,
                    "User registration failed",
                    "USER_REGISTRATION_FAILED",
                    ex);
            }
        }


        public async Task<AuthLoginResponseDto> LoginAsync(AuthLoginDto loginDto)
        {
            try
            {
                ValidateAuthDto(loginDto);
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
            catch (Exception ex) when (ex is not ApiException)
            {
                _logger.LogError(ex, "Error during user login: {EmailOrUsername}", loginDto.EmailOrUserName);
                throw new ApiException(
                    (int)HttpStatusCode.Unauthorized,
                    "Login failed",
                    "LOGIN_FAILED",
                    ex);
            }
        }

        public async Task<UserStatusResponseDto> BlockUserAsync(string userId)
        {
            try
            {
                ValidateUserId(userId);
                _logger.LogInformation("Attempting to block user: {UserId}", userId);

                var user = await _unitOfWork.AuthRepository.BlockUserAsync(userId);

                var response = _mapper.Map<UserStatusResponseDto>(user);
                response.IsBlocked = true; 

                _logger.LogInformation("User blocked successfully: {UserId}", userId);
                return response;
            }
            catch (Exception ex) when (ex is not ApiException)
            {
                _logger.LogError(ex, "Error blocking user: {UserId}", userId);
                throw new ApiException(
                    (int)HttpStatusCode.InternalServerError,
                    "Failed to block user",
                    "BLOCK_USER_FAILED",
                    ex);
            }
        }

        public async Task<UserStatusResponseDto> UnblockUserAsync(string userId)
        {
            try
            {
                ValidateUserId(userId);
                _logger.LogInformation("Attempting to unblock user: {UserId}", userId);

                var user = await _unitOfWork.AuthRepository.UnblockUserAsync(userId);

                var response = _mapper.Map<UserStatusResponseDto>(user);
                response.IsBlocked = false;

                _logger.LogInformation("User unblocked successfully: {UserId}", userId);
                return response;
            }
            catch (Exception ex) when (ex is not ApiException)
            {
                _logger.LogError(ex, "Error unblocking user: {UserId}", userId);
                throw new ApiException(
                    (int)HttpStatusCode.InternalServerError,
                    "Failed to unblock user",
                    "UNBLOCK_USER_FAILED",
                    ex);
            }
        }

        public async Task<string> GeneratePasswordResetTokenAsync(ForgotPasswordDto forgotPasswordDto)
        {
            try
            {
                ValidateEmail(forgotPasswordDto?.Email);
                _logger.LogInformation("Generating password reset token for email: {Email}", forgotPasswordDto.Email);

                var token = await _unitOfWork.AuthRepository.GeneratePasswordResetTokenAsync(forgotPasswordDto.Email);
                var resetLink = $"{_configuration["App:BaseUrl"]}/api/auth/reset-password?email={forgotPasswordDto.Email}&token={Uri.EscapeDataString(token)}";

                await SendPasswordResetEmailAsync(forgotPasswordDto.Email, resetLink);

                _logger.LogInformation("Password reset token generated successfully for email: {Email}", forgotPasswordDto.Email);
                
                return token;
            }
            catch (Exception ex) when (ex is not ApiException)
            {
                _logger.LogError(ex, "Error generating password reset token for email: {Email}", forgotPasswordDto?.Email);
                throw new ApiException(
                    (int)HttpStatusCode.InternalServerError,
                    "Failed to generate password reset token",
                    "PASSWORD_RESET_TOKEN_FAILED",
                    ex);
            }
        }

        public async Task<IdentityResult> ResetPasswordAsync(ResetPasswordDto resetPasswordDto)
        {
            try
            {
                ValidateResetPasswordDto(resetPasswordDto);
                _logger.LogInformation("Attempting to reset password for email: {Email}", resetPasswordDto.Email);

                var result = await _unitOfWork.AuthRepository.ResetPasswordAsync(resetPasswordDto);

                if (result.Succeeded)
                {
                    _logger.LogInformation("Password reset successful for email: {Email}", resetPasswordDto.Email);
                }
                else
                {
                    _logger.LogWarning("Password reset failed for email {Email}: {Errors}",
                        resetPasswordDto.Email,
                        string.Join(", ", result.Errors.Select(e => e.Description)));
                }

                return result;
            }
            catch (Exception ex) when (ex is not ApiException)
            {
                _logger.LogError(ex, "Error resetting password for email: {Email}", resetPasswordDto.Email);
                throw new ApiException(
                    (int)HttpStatusCode.InternalServerError,
                    "Password reset failed",
                    "PASSWORD_RESET_FAILED",
                    ex);
            }
        }

        public async Task LogoutAsync()
        {
            try
            {
                _logger.LogInformation("Starting logout process");
                await _unitOfWork.AuthRepository.LogoutAsync();
                _logger.LogInformation("Logout completed successfully");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during logout");
                throw new ApiException(
                    (int)HttpStatusCode.InternalServerError,
                    "Logout failed",
                    "LOGOUT_FAILED",
                    ex);
            }
        }

        #region Private Methods

        private async Task SendPasswordResetEmailAsync(string email, string resetLink)
        {
            try
            {
                ValidateEmail(email);
                _logger.LogDebug("Preparing password reset email for: {Email}", email);

                var subject = "Password Reset Request";
                var body = $"Please reset your password by clicking <a href='{resetLink}'>here</a>";

                await _unitOfWork.AuthEmailService.SendEmailAsync(email, subject, body);
                _logger.LogInformation("Password reset email sent to: {Email}", email);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error sending password reset email to: {Email}", email);
                throw new ApiException(
                    (int)HttpStatusCode.InternalServerError,
                    "Failed to send password reset email",
                    "EMAIL_SEND_FAILED",
                    ex);
            }
        }

        #endregion

        #region Validation Methods

        private void ValidateAuthDto(object dto)
        {
            if (dto == null)
            {
                throw new LangLearningAPI.Exceptions.ValidationException(
                    "Request data cannot be null",
                    "NULL_REQUEST");
            }

            var validationResults = new List<ValidationResult>();
            var context = new ValidationContext(dto);

            if (!Validator.TryValidateObject(dto, context, validationResults, true))
            {
                var errors = validationResults
                    .GroupBy(v => v.MemberNames.FirstOrDefault() ?? "")
                    .ToDictionary(
                        g => g.Key,
                        g => g.Select(v => v.ErrorMessage ?? "").ToArray());

                throw new LangLearningAPI.Exceptions.ValidationException(
                    "Validation failed",
                    "VALIDATION_ERROR",
                    errors);
            }
        }

        private void ValidateUserId(string userId)
        {
            if (string.IsNullOrWhiteSpace(userId))
            {
                throw new LangLearningAPI.Exceptions.ValidationException(
                    "User ID cannot be empty",
                    "EMPTY_USER_ID");
            }
        }

        private void ValidateEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
            {
                throw new LangLearningAPI.Exceptions.ValidationException(
                    "Email cannot be empty",
                    "EMPTY_EMAIL");
            }

            if (!new EmailAddressAttribute().IsValid(email))
            {
                throw new LangLearningAPI.Exceptions.ValidationException(
                    "Invalid email format",
                    "INVALID_EMAIL");
            }
        }

        private void ValidateResetPasswordDto(ResetPasswordDto dto)
        {
            if (dto == null)
            {
                throw new LangLearningAPI.Exceptions.ValidationException(
                    "Reset password data cannot be null",
                    "NULL_RESET_DATA");
            }

            ValidateEmail(dto.Email);

            if (string.IsNullOrWhiteSpace(dto.Token))
            {
                throw new LangLearningAPI.Exceptions.ValidationException(
                    "Token cannot be empty",
                    "EMPTY_TOKEN");
            }

            if (string.IsNullOrWhiteSpace(dto.NewPassword))
            {
                throw new LangLearningAPI.Exceptions.ValidationException(
                    "New password cannot be empty",
                    "EMPTY_PASSWORD");
            }
        }

        #endregion
    }
}