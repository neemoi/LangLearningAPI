﻿using Application.Services.Interfaces.IRepository.Profile;
using Domain.Models;
using LangLearningAPI.Exceptions;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Net;

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
                if (string.IsNullOrWhiteSpace(id))
                {
                    _logger.LogWarning("Attempted to get user with empty ID");
                    throw new ArgumentException("User ID cannot be empty", nameof(id));
                }

                _logger.LogInformation("Fetching user by ID: {UserId}", id);

                var user = await _userManager.FindByIdAsync(id);

                if (user == null)
                {
                    _logger.LogWarning("User with ID {UserId} not found", id);
                    throw new NotFoundException($"User with ID {id} not found", "USER_NOT_FOUND");
                }

                return user;
            }
            catch (Exception ex) when (ex is not ApiException)
            {
                _logger.LogError(ex, "Error fetching user by ID: {UserId}", id);
                throw new ApiException(
                    (int)HttpStatusCode.InternalServerError,
                    "Failed to get user",
                    "GET_USER_FAILED",
                    ex);
            }
        }

        public async Task<IEnumerable<Users>> GetAllUsersAsync()
        {
            try
            {
                _logger.LogInformation("Fetching all users");

                var users = await _userManager.Users.ToListAsync();

                if (users == null || !users.Any())
                {
                    _logger.LogWarning("No users found in database");
                    return Enumerable.Empty<Users>();
                }

                return users;
            }
            catch (Exception ex) when (ex is not ApiException)
            {
                _logger.LogError(ex, "Error fetching all users");
                throw new ApiException(
                    (int)HttpStatusCode.InternalServerError,
                    "Failed to get users",
                    "GET_USERS_FAILED",
                    ex);
            }
        }

        public async Task<IdentityResult> UpdateUserAsync(Users user)
        {
            try
            {
                if (user == null)
                {
                    _logger.LogWarning("Attempted to update null user");
                    throw new ArgumentNullException(nameof(user), "User cannot be null");
                }

                _logger.LogInformation("Updating user with ID: {UserId}", user.Id);

                var result = await _userManager.UpdateAsync(user);

                if (!result.Succeeded)
                {
                    var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                    _logger.LogWarning("Failed to update user {UserId}. Errors: {Errors}", user.Id, errors);
                    throw new IdentityException("User update failed", "USER_UPDATE_FAILED", result.Errors.ToString());
                }

                _logger.LogInformation("User {UserId} updated successfully", user.Id);
                
                return result;
            }
            catch (Exception ex) when (ex is not ApiException)
            {
                _logger.LogError(ex, "Error updating user with ID: {UserId}", user?.Id);
                throw new ApiException(
                    (int)HttpStatusCode.InternalServerError,
                    "Failed to update user",
                    "UPDATE_USER_FAILED",
                    ex);
            }
        }

        public async Task<IdentityResult> DeleteUserAsync(string id)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(id))
                {
                    _logger.LogWarning("Attempted to delete user with empty ID");
                    throw new ArgumentException("User ID cannot be empty", nameof(id));
                }

                _logger.LogInformation("Deleting user with ID: {UserId}", id);

                var user = await _userManager.FindByIdAsync(id);
                if (user == null)
                {
                    _logger.LogWarning("User with ID {UserId} not found for deletion", id);
                    throw new NotFoundException($"User with ID {id} not found", "USER_NOT_FOUND");
                }

                var result = await _userManager.DeleteAsync(user);

                if (!result.Succeeded)
                {
                    var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                    _logger.LogWarning("Failed to delete user {UserId}. Errors: {Errors}", id, errors);
                    throw new IdentityException("User deletion failed", "USER_DELETION_FAILED", result.Errors.ToString());
                }

                _logger.LogInformation("User {UserId} deleted successfully", id);
                
                return result;
            }
            catch (Exception ex) when (ex is not ApiException)
            {
                _logger.LogError(ex, "Error deleting user with ID: {UserId}", id);
                throw new ApiException(
                    (int)HttpStatusCode.InternalServerError,
                    "Failed to delete user",
                    "DELETE_USER_FAILED",
                    ex);
            }
        }
    }
}