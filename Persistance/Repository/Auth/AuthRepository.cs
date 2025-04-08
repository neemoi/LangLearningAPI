using Application.DtoModels.Auth;
using Application.Services.Interfaces.IRepository.Auth;
using AutoMapper;
using Domain.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using System.ComponentModel.DataAnnotations;

public class AuthRepository : IAuthRepository
{
    private readonly UserManager<Users> _userManager;
    private readonly SignInManager<Users> _signInManager;
    private readonly ILogger<AuthRepository> _logger;
    private readonly IMapper _mapper;

    public AuthRepository(UserManager<Users> userManager, ILogger<AuthRepository> logger, IMapper mapper, SignInManager<Users> signInManager)
    {
        _userManager = userManager;
        _logger = logger;
        _mapper = mapper;
        _signInManager = signInManager;
    }

    public async Task<Users> RegisterUserAsync(AuthRegisterDto registerDto)
    {
        if (registerDto == null)
        {
            throw new ArgumentNullException(nameof(registerDto), "RegisterDto cannot be null");
        }

        var validationContext = new ValidationContext(registerDto);
        var validationResults = new List<ValidationResult>();
        bool isValid = Validator.TryValidateObject(registerDto, validationContext, validationResults, true);

        if (!isValid)
        {
            var errors = validationResults.Select(vr => vr.ErrorMessage).ToList();
            throw new ValidationException(string.Join(", ", errors));
        }

        try
        {
            _logger.LogInformation("Admin is attempting to register a new user: {Email}", registerDto.Email);

            var user = _mapper.Map<Users>(registerDto);

            if (await _userManager.FindByEmailAsync(registerDto.Email) != null)
            {
                throw new Exception($"User with email {registerDto.Email} already exists");
            }

            var result = await _userManager.CreateAsync(user, registerDto.Password);
            if (!result.Succeeded)
            {
                throw new Exception(string.Join(", ", result.Errors.Select(e => e.Description)));
            }

            await _userManager.AddToRoleAsync(user, "User");

            _logger.LogInformation("User {Email} successfully registered", registerDto.Email);
            return user;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error registering user: {Email}", registerDto.Email);
            throw;
        }
    }

    public async Task<Users> LoginAsync(AuthLoginDto loginDto)
    {
        if (loginDto == null)
        {
            throw new ArgumentNullException(nameof(loginDto), "LoginDto cannot be null");
        }

        var validationContext = new ValidationContext(loginDto);
        var validationResults = new List<ValidationResult>();
        bool isValid = Validator.TryValidateObject(loginDto, validationContext, validationResults, true);

        if (!isValid)
        {
            var errors = validationResults.Select(vr => vr.ErrorMessage).ToList();
            throw new ValidationException(string.Join(", ", errors));
        }

        try
        {
            _logger.LogInformation("User attempting to log in: {EmailOrUsername}", loginDto.EmailOrUserName);

            var user = await _userManager.FindByEmailAsync(loginDto.EmailOrUserName);

            if (user == null)
            {
                user = await _userManager.FindByNameAsync(loginDto.EmailOrUserName);
            }

            if (user == null)
            {
                throw new Exception("Invalid email/username or password");
            }

            if (!await _userManager.CheckPasswordAsync(user, loginDto.Password))
            {
                throw new Exception("Invalid email/username or password");
            }

            if (user.LockoutEnd != null && user.LockoutEnd > DateTime.UtcNow)
            {
                throw new Exception("User is blocked");
            }

            _logger.LogInformation("User {EmailOrUsername} successfully logged in", loginDto.EmailOrUserName);
            return user;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Login failed for: {EmailOrUsername}", loginDto.EmailOrUserName);
            throw;
        }
    }

    public async Task<Users> BlockUserAsync(string userId)
    {
        if (string.IsNullOrEmpty(userId))
        {
            throw new ArgumentNullException(nameof(userId), "UserId cannot be null or empty");
        }

        try
        {
            _logger.LogInformation("Attempting to block user: {UserId}", userId);

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                throw new Exception("User not found");
            }

            user.LockoutEnd = DateTimeOffset.MaxValue;
            await _userManager.UpdateAsync(user);

            _logger.LogInformation("User {UserId} has been blocked", userId);
            return user;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error blocking user: {UserId}", userId);
            throw;
        }
    }

    public async Task<Users> UnblockUserAsync(string userId)
    {
        if (string.IsNullOrEmpty(userId))
        {
            throw new ArgumentNullException(nameof(userId), "UserId cannot be null or empty");
        }

        try
        {
            _logger.LogInformation("Attempting to unblock user: {UserId}", userId);

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                throw new Exception("User not found");
            }

            user.LockoutEnd = null;
            await _userManager.UpdateAsync(user);

            _logger.LogInformation("User {UserId} has been unblocked", userId);
            return user;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error unblocking user: {UserId}", userId);
            throw;
        }
    }

    public async Task<string> GeneratePasswordResetTokenAsync(string email)
    {
        if (string.IsNullOrEmpty(email))
        {
            throw new ArgumentNullException(nameof(email), "Email cannot be null or empty");
        }

        try
        {
            _logger.LogInformation("Generating password reset token for email: {Email}", email);

            var user = await _userManager.FindByEmailAsync(email)
                       ?? throw new Exception($"User with email {email} not found");

            var token = await _userManager.GeneratePasswordResetTokenAsync(user);

            return token;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error generating password reset token for email: {Email}", email);
            throw;
        }
    }

    public async Task<IdentityResult> ResetPasswordAsync(ResetPasswordDto resetPasswordDto)
    {
        if (resetPasswordDto == null)
        {
            throw new ArgumentNullException(nameof(resetPasswordDto), "ResetPasswordDto cannot be null");
        }

        var validationContext = new ValidationContext(resetPasswordDto);
        var validationResults = new List<ValidationResult>();
        bool isValid = Validator.TryValidateObject(resetPasswordDto, validationContext, validationResults, true);

        if (!isValid)
        {
            var errors = validationResults.Select(vr => vr.ErrorMessage).ToList();
            throw new ValidationException(string.Join(", ", errors));
        }

        try
        {
            _logger.LogInformation("Attempting to reset password for email: {Email}", resetPasswordDto.Email);

            var user = await _userManager.FindByEmailAsync(resetPasswordDto.Email)
                       ?? throw new Exception($"User with email {resetPasswordDto.Email} not found");

            var result = await _userManager.ResetPasswordAsync(user, resetPasswordDto.Token, resetPasswordDto.NewPassword);

            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error resetting password for email: {Email}", resetPasswordDto.Email);
            throw;
        }
    }

    public async Task LogoutAsync()
    {
        try
        {
            _logger.LogInformation("Logging out user.");
            await _signInManager.SignOutAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred during logout");
            throw;
        }
    }
}