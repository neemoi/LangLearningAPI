using Application.DtoModels.AdminUsers;
using Application.DtoModels.User;
using Application.Services.Interfaces.IServices;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

[ApiController]
[Route("api/[controller]")]
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
    public async Task<ActionResult<IEnumerable<UserDto>>> GetAllUsers()
    {
        try
        {
            _logger.LogInformation("Fetching all users.");
            var users = await _userService.GetAllUsersAsync();
            return Ok(users);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error fetching all users.");
            return StatusCode(500, "An error occurred while fetching users.");
        }
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<UserDto>> GetUserById(string id)
    {
        try
        {
            _logger.LogInformation($"Fetching user by ID: {id}");
            var user = await _userService.GetUserByIdAsync(id);
            if (user == null)
            {
                return NotFound("User not found.");
            }
            return Ok(user);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error fetching user by ID: {id}");
            return StatusCode(500, "An error occurred while fetching the user.");
        }
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateUser(string id, [FromBody] UpdateUserDto updateUserDto)
    {
        try
        {
            _logger.LogInformation($"Updating user with ID: {id}");
            return Ok(await _userService.UpdateUserAsync(id, updateUserDto));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error updating user with ID: {id}");
            return StatusCode(500, "An error occurred while updating the user.");
        }
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteUser(string id, string currentUserId)
    {
        try
        {
            _logger.LogInformation($"Deleting user with ID: {id}");
            return Ok(await _userService.DeleteUserAsync(id, currentUserId));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error deleting user with ID: {id}");
            return StatusCode(500, "An error occurred while deleting the user.");
        }
    }
}