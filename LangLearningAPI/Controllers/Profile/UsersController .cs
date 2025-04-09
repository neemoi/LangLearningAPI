using Application.DtoModels.AdminUsers;
using Application.DtoModels.User;
using Application.Services.Interfaces.IServices.Profile;
using LangLearningAPI.Exceptions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LangLearningAPI.Controllers
{
    [ApiController]
    [Route("api/users")]
    [Produces("application/json")]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly ILogger<UsersController> _logger;

        public UsersController(IUserService userService, ILogger<UsersController> logger)
        {
            _userService = userService;
            _logger = logger;
        }

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<UserDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetAllUsers()
        {
            try
            {
                _logger.LogInformation("Fetching all users");

                return Ok(await _userService.GetAllUsersAsync());
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching all users");
                return CreateProblemResponse(
                    StatusCodes.Status500InternalServerError,
                    "Failed to get users",
                    "GET_USERS_FAILED",
                    ex);
            }
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(UserDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetUserById(string id)
        {
            try
            {
                _logger.LogInformation("Fetching user by ID: {UserId}", id);
                var user = await _userService.GetUserByIdAsync(id);

                if (user == null)
                {
                    _logger.LogWarning("User with ID {UserId} not found", id);
                    return NotFound(CreateProblemDetails(
                        StatusCodes.Status404NotFound,
                        "User not found",
                        $"User with ID {id} not found"));
                }

                return Ok(user);
            }
            catch (NotFoundException ex)
            {
                _logger.LogWarning(ex, "User not found: {UserId}", id);
                return NotFound(CreateProblemDetails(ex));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching user by ID: {UserId}", id);
                return CreateProblemResponse(
                    StatusCodes.Status500InternalServerError,
                    "Failed to get user",
                    "GET_USER_FAILED",
                    ex);
            }
        }

        [HttpPut("{id}")]
        [ProducesResponseType(typeof(UserResponseDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdateUser(string id, [FromBody] UpdateUserDto updateUserDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    _logger.LogWarning("Invalid model state for user update");
                    return BadRequest(ModelState);
                }

                _logger.LogInformation("Updating user with ID: {UserId}", id);

                return Ok(await _userService.UpdateUserAsync(id, updateUserDto));
            }
            catch (NotFoundException ex)
            {
                _logger.LogWarning(ex, "User not found for update: {UserId}", id);
                return NotFound(CreateProblemDetails(ex));
            }
            catch (ValidationException ex)
            {
                _logger.LogWarning(ex, "Validation error updating user: {UserId}", id);
                return BadRequest(CreateProblemDetails(ex));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating user with ID: {UserId}", id);
                return CreateProblemResponse(
                    StatusCodes.Status500InternalServerError,
                    "Failed to update user",
                    "UPDATE_USER_FAILED",
                    ex);
            }
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(typeof(UserResponseDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeleteUser(string id, [FromQuery] string currentUserId)
        {
            try
            {
                _logger.LogInformation("Deleting user with ID: {UserId}", id);

                return Ok(await _userService.DeleteUserAsync(id, currentUserId));
            }
            catch (NotFoundException ex)
            {
                _logger.LogWarning(ex, "User not found for deletion: {UserId}", id);
                return NotFound(CreateProblemDetails(ex));
            }
            catch (ForbiddenException ex)
            {
                _logger.LogWarning(ex, "Forbidden to delete user: {UserId}", id);
                return StatusCode(StatusCodes.Status403Forbidden, CreateProblemDetails(ex));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting user with ID: {UserId}", id);
                return CreateProblemResponse(
                    StatusCodes.Status500InternalServerError,
                    "Failed to delete user",
                    "DELETE_USER_FAILED",
                    ex);
            }
        }

        private IActionResult CreateProblemResponse(int statusCode, string title, string errorCode, Exception ex)
        {
            return StatusCode(statusCode, CreateProblemDetails(statusCode, title, ex.Message, errorCode));
        }

        private static ProblemDetails CreateProblemDetails(int statusCode, string title, string detail, string errorCode = null)
        {
            var problemDetails = new ProblemDetails
            {
                Status = statusCode,
                Title = title,
                Detail = detail
            };

            if (!string.IsNullOrEmpty(errorCode))
            {
                problemDetails.Extensions["errorCode"] = errorCode;
            }

            return problemDetails;
        }

        private static ProblemDetails CreateProblemDetails(ApiException ex)
        {
            return new ProblemDetails
            {
                Status = ex.StatusCode,
                Title = ex.Message,
                Detail = ex.InnerException?.Message,
                Extensions = { ["errorCode"] = ex.ErrorCode }
            };
        }
    }
}