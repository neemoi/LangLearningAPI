using Application.DtoModels.Auth;
using Application.Services.Interfaces.IRepository.Auth;
using AutoMapper;
using Domain.Models;
using LangLearningAPI.Exceptions;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using System.ComponentModel.DataAnnotations;
using System.Net;

namespace Persistance.Repository.Auth
{
    public class AuthRepository : IAuthRepository
    {
        private readonly UserManager<Users> _userManager;
        private readonly SignInManager<Users> _signInManager;
        private readonly ILogger<AuthRepository> _logger;
        private readonly IMapper _mapper;

        public AuthRepository(UserManager<Users> userManager, SignInManager<Users> signInManager,
            ILogger<AuthRepository> logger, IMapper mapper)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task<Users> RegisterUserAsync(AuthRegisterDto registerDto)
        {
            try
            {
                ValidateAuthDto(registerDto);

                await ValidateUserNotExists(registerDto.Email);

                _logger.LogInformation("Registering new user: {Email}", registerDto.Email);

                var user = _mapper.Map<Users>(registerDto);
                var result = await _userManager.CreateAsync(user, registerDto.Password);

                if (!result.Succeeded)
                {
                    foreach (var error in result.Errors)
                    {
                        _logger.LogError("Registration failed: {Error}", error.Description);
                    }
                    throw new IdentityException(
                        "User creation failed",
                        "USER_CREATION_FAILED",
                        result.Errors.Select(e => e.Description).ToArray());
                }

                await _userManager.AddToRoleAsync(user, "User");

                _logger.LogInformation("User {Email} registered successfully", registerDto.Email);

                return user;
            }
            catch (Exception ex) when (ex is not ApiException)
            {
                _logger.LogError(ex, "Error registering user {Email}", registerDto.Email);
                throw new ApiException(
                    (int)HttpStatusCode.InternalServerError,
                    "User registration failed",
                    "USER_REGISTRATION_FAILED",
                    ex);
            }
        }

        public async Task<Users> LoginAsync(AuthLoginDto loginDto)
        {
            try
            {
                ValidateAuthDto(loginDto);

                _logger.LogInformation("Login attempt for: {EmailOrUsername}", loginDto.EmailOrUserName);

                var user = await FindUserByEmailOrUsername(loginDto.EmailOrUserName)
                    ?? throw new NotFoundException("User not found", "USER_NOT_FOUND");

                await ValidateUserCredentials(user, loginDto.Password);
                CheckUserBlockedStatus(user);

                _logger.LogInformation("User {EmailOrUsername} logged in successfully", loginDto.EmailOrUserName);
                
                return user;
            }
            catch (Exception ex) when (ex is not ApiException)
            {
                _logger.LogError(ex, "Login failed for: {EmailOrUsername}", loginDto.EmailOrUserName);
                throw new ApiException(
                    (int)HttpStatusCode.Unauthorized,
                    "Login failed",
                    "LOGIN_FAILED",
                    ex);
            }
        }

        public async Task<Users> BlockUserAsync(string userId)
        {
            try
            {
                ValidateUserId(userId);
                _logger.LogInformation("Blocking user: {UserId}", userId);

                var user = await GetUserById(userId);
                user.LockoutEnd = DateTimeOffset.MaxValue;
                user.LockoutEnabled = true; 

                var result = await _userManager.UpdateAsync(user);
                if (!result.Succeeded)
                {
                    throw new IdentityException(
                        "Failed to update user lockout",
                        "LOCKOUT_UPDATE_FAILED",
                        result.Errors.Select(e => e.Description).ToArray());
                }

                _logger.LogInformation("User {UserId} blocked successfully", userId);
                return user;
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

        public async Task<Users> UnblockUserAsync(string userId)
        {
            try
            {
                ValidateUserId(userId);

                _logger.LogInformation("Unblocking user: {UserId}", userId);

                var user = await GetUserById(userId);
                user.LockoutEnd = null;

                await _userManager.UpdateAsync(user);

                _logger.LogInformation("User {UserId} unblocked successfully", userId);
                
                return user;
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

        public async Task<string> GeneratePasswordResetTokenAsync(string email)
        {
            try
            {
                ValidateEmail(email);

                _logger.LogInformation("Generating password reset token for: {Email}", email);

                var user = await GetUserByEmail(email);
                var token = await _userManager.GeneratePasswordResetTokenAsync(user);

                _logger.LogInformation("Password reset token generated for: {Email}", email);
                
                return token;
            }
            catch (Exception ex) when (ex is not ApiException)
            {
                _logger.LogError(ex, "Error generating password reset token for: {Email}", email);
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
                ValidateAuthDto(resetPasswordDto);

                _logger.LogInformation("Resetting password for: {Email}", resetPasswordDto.Email);

                var user = await GetUserByEmail(resetPasswordDto.Email);
                var result = await _userManager.ResetPasswordAsync(
                    user,
                    resetPasswordDto.Token,
                    resetPasswordDto.NewPassword);

                if (!result.Succeeded)
                {
                    throw new IdentityException(
                        "Password reset failed",
                        "PASSWORD_RESET_FAILED",
                        result.Errors.Select(e => e.Description).ToArray());
                }

                _logger.LogInformation("Password reset successfully for: {Email}", resetPasswordDto.Email);
                
                return result;
            }
            catch (Exception ex) when (ex is not ApiException)
            {
                _logger.LogError(ex, "Error resetting password for: {Email}", resetPasswordDto.Email);
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
                _logger.LogInformation("Logging out current user");
                
                await _signInManager.SignOutAsync();
                
                _logger.LogInformation("User logged out successfully");
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

        #region Private Helpers

        private async Task<Users> GetUserById(string userId)
        {
            return await _userManager.FindByIdAsync(userId)
                ?? throw new NotFoundException($"User with ID {userId} not found", "USER_NOT_FOUND");
        }

        private async Task<Users> GetUserByEmail(string email)
        {
            return await _userManager.FindByEmailAsync(email)
                ?? throw new NotFoundException($"User with email {email} not found", "USER_NOT_FOUND");
        }

        private async Task<Users> FindUserByEmailOrUsername(string emailOrUsername)
        {
            var user = await _userManager.FindByEmailAsync(emailOrUsername);
            return user ?? await _userManager.FindByNameAsync(emailOrUsername);
        }

        private async Task ValidateUserCredentials(Users user, string password)
        {
            if (!await _userManager.CheckPasswordAsync(user, password))
            {
                throw new UnauthorizedException("Invalid credentials", "INVALID_CREDENTIALS");
            }
        }

        private void CheckUserBlockedStatus(Users user)
        {
            if (user.LockoutEnd != null && user.LockoutEnd > DateTimeOffset.UtcNow)
            {
                throw new ForbiddenException("User is blocked", "USER_BLOCKED");
            }
        }

        private async Task ValidateUserNotExists(string email)
        {
            if (await _userManager.FindByEmailAsync(email) != null)
            {
                throw new ConflictException($"User with email {email} already exists", "USER_ALREADY_EXISTS");
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
        
        #endregion
    }
}