using Application.DtoModels.Auth;
using Application.Services.Interfaces.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LangLearningAPI.Controllers.Auth
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly ILogger<AuthController> _logger;

        public AuthController(IAuthService authService, ILogger<AuthController> logger)
        {
            _authService = authService;
            _logger = logger;
        }

        [HttpPost("register")]
        public async Task<IActionResult> RegisterUserAsync([FromBody] AuthRegisterDto model)
        {
            try
            {
                return Ok(await _authService.RegisterUserAsync(model));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred during user registration: {Email}", model.Email);
                return BadRequest(new { error = ex.Message });
            }
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<IActionResult> LoginAsync([FromBody] AuthLoginDto model)
        {
            try
            {
                return Ok(await _authService.LoginAsync(model));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred during login: {Email}", model.EmailOrUserName);
                return Unauthorized(new { error = ex.Message });
            }
        }


        [HttpPost("block-user/{userId}")]
        public async Task<IActionResult> BlockUserAsync(string userId)
        {
            try
            {
                return Ok(await _authService.BlockUserAsync(userId));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while blocking user: {UserId}", userId);
                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpPost("unblock-user/{userId}")]
        public async Task<IActionResult> UnBlockUserAsync(string userId)
        {
            try
            {
                return Ok(await _authService.UnblockUserAsync(userId)); 
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while unblocking user: {UserId}", userId);
                return BadRequest(new { error = ex.Message });
            }
        }


        [HttpPost("forgot-password")]
        [AllowAnonymous]
        public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordDto forgotPasswordDto)
        {
            try
            {
                var token = await _authService.GeneratePasswordResetTokenAsync(forgotPasswordDto);
                return Ok(new { Token = token });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error generating password reset token for email: {Email}", forgotPasswordDto.Email);
                return HandleException(ex);
            }
        }

        [HttpPost("reset-password")]
        [AllowAnonymous]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordDto resetPasswordDto)
        {
            try
            {
                return Ok(await _authService.ResetPasswordAsync(resetPasswordDto));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred during password reset for email: {Email}", resetPasswordDto.Email);
                return HandleException(ex);
            }
        }

        [HttpPost("logout")]
        public async Task<IActionResult> Logout()
        {
            try
            {
                await _authService.LogoutAsync();
                return Ok("Logout successful");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred during logout");
                return HandleException(ex);
            }
        }

        private IActionResult HandleException(Exception ex)
        {
            _logger.LogError(ex, "An error occurred in AuthController: {Message}", ex.Message);
            return StatusCode(500, new { Message = "An internal server error occurred.", Details = ex.Message });
        }
    }
}
