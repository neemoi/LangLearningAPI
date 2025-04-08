using Application.Services.Interfaces.IRepository.Profile;
using Domain.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Persistance.Repository.Userfsf
{
    public class UserRepository : IUserRepository
    {
        private readonly UserManager<Users> _userManager;
        private readonly ILogger<UserRepository> _logger;

        public UserRepository(UserManager<Users> userManager, ILogger<UserRepository> logger)
        {
            _userManager = userManager;
            _logger = logger;
        }

        public async Task<Users> GetUserByIdAsync(string id)
        {
            try
            {
                _logger.LogInformation($"Fetching user by ID: {id}");

                var user = await _userManager.FindByIdAsync(id);

                if (user == null)
                {
                    _logger.LogWarning($"User with ID {id} not found.");

                    return null;
                }

                return user;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error fetching user by ID: {id}");
                throw;
            }
        }

        public async Task<IEnumerable<Users>> GetAllUsersAsync()
        {
            try
            {
                _logger.LogInformation("Fetching all users.");

                return await _userManager.Users.ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching all users.");
                throw;
            }
        }

        public async Task<IdentityResult> UpdateUserAsync(Users user)
        {
            try
            {
                _logger.LogInformation($"Updating user with ID: {user.Id}");

                if (user == null)
                {
                    _logger.LogWarning("User object is null.");

                    return IdentityResult.Failed(new IdentityError { Description = "User object is null." });
                }

                var result = await _userManager.UpdateAsync(user);

                if (!result.Succeeded)
                {
                    _logger.LogWarning($"Failed to update user with ID {user.Id}: {string.Join(", ", result.Errors.Select(e => e.Description))}");
                }
                else
                {
                    _logger.LogInformation($"User with ID {user.Id} updated successfully.");
                }
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error updating user with ID: {user?.Id}");
                throw;
            }
        }

        public async Task<IdentityResult> DeleteUserAsync(string id)
        {
            try
            {
                _logger.LogInformation($"Deleting user with ID: {id}");

                var user = await _userManager.FindByIdAsync(id);

                if (user == null)
                {
                    _logger.LogWarning($"User with ID {id} not found.");

                    return IdentityResult.Failed(new IdentityError { Description = "User not found." });
                }

                var result = await _userManager.DeleteAsync(user);

                if (!result.Succeeded)
                {
                    _logger.LogWarning($"Failed to delete user with ID {id}: {string.Join(", ", result.Errors.Select(e => e.Description))}");
                }
                else
                {
                    _logger.LogInformation($"User with ID {id} deleted successfully.");
                }
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error deleting user with ID: {id}");
                throw;
            }
        }
    }
}