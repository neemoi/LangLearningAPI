using Application.DtoModels.AdminUsers;
using Application.DtoModels.User;
using Application.Services.Interfaces.IServices.Profile;
using Application.UnitOfWork;
using AutoMapper;
using Domain.Models;
using LangLearningAPI.Exceptions;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using System.Net;

namespace Application.Services.Implementations
{
    public class UserService : IUserService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger<UserService> _logger;
        private readonly UserManager<Users> _userManager;

        public UserService(IUnitOfWork unitOfWork, IMapper mapper,
            ILogger<UserService> logger, UserManager<Users> userManager)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logger = logger;
            _userManager = userManager;
        }

        public async Task<UserByIdDto> GetUserByIdAsync(string id)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(id))
                {
                    throw new ArgumentException("User ID cannot be empty", nameof(id));
                }

                _logger.LogInformation("Fetching user by ID: {UserId}", id);

                var user = await _unitOfWork.UserRepository.GetUserByIdAsync(id);

                if (user == null)
                {
                    _logger.LogWarning("User with ID {UserId} not found", id);
                    throw new NotFoundException($"User with ID {id} not found", "USER_NOT_FOUND");
                }

                return _mapper.Map<UserByIdDto>(user);
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

        public async Task<IEnumerable<UserDto>> GetAllUsersAsync()
        {
            try
            {
                _logger.LogInformation("Fetching all users");

                var users = await _unitOfWork.UserRepository.GetAllUsersAsync();
                var userDtos = new List<UserDto>();

                foreach (var user in users)
                {
                    var userDto = _mapper.Map<UserDto>(user);
                    var roles = await _userManager.GetRolesAsync(user);
                    userDto.Role = roles.FirstOrDefault();
                    userDtos.Add(userDto);
                }

                return userDtos;
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

        public async Task<UserResponseDto> UpdateUserAsync(string id, UpdateUserDto updateUserDto)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(id))
                {
                    throw new ArgumentException("User ID cannot be empty", nameof(id));
                }

                _logger.LogInformation("Updating user with ID: {UserId}", id);

                var user = await _unitOfWork.UserRepository.GetUserByIdAsync(id);

                if (user == null)
                {
                    _logger.LogWarning("User with ID {UserId} not found", id);
                    throw new NotFoundException($"User with ID {id} not found", "USER_NOT_FOUND");
                }

                _mapper.Map(updateUserDto, user);

                var result = await _unitOfWork.UserRepository.UpdateUserAsync(user);

                if (!result.Succeeded)
                {
                    var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                    _logger.LogWarning("Failed to update user {UserId}. Errors: {Errors}", id, errors);
                    throw new IdentityException("User update failed", "USER_UPDATE_FAILED", result.Errors.ToString());
                }

                var userDto = _mapper.Map<UserResponseDto>(user);
                var roles = await _userManager.GetRolesAsync(user);
                userDto.Role = roles.FirstOrDefault();

                _logger.LogInformation("User {UserId} updated successfully", id);
                
                return userDto;
            }
            catch (Exception ex) when (ex is not ApiException)
            {
                _logger.LogError(ex, "Error updating user with ID: {UserId}", id);
                throw new ApiException(
                    (int)HttpStatusCode.InternalServerError,
                    "Failed to update user",
                    "UPDATE_USER_FAILED",
                    ex);
            }
        }

        public async Task<UserResponseDto> DeleteUserAsync(string id, string currentUserId)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(id))
                {
                    throw new ArgumentException("User ID cannot be empty", nameof(id));
                }

                if (string.IsNullOrWhiteSpace(currentUserId))
                {
                    throw new ArgumentException("Current user ID cannot be empty", nameof(currentUserId));
                }

                _logger.LogInformation("Deleting user with ID: {UserId}", id);

                var currentUser = await _userManager.FindByIdAsync(currentUserId);

                if (currentUser == null)
                {
                    _logger.LogWarning("Current user with ID {CurrentUserId} not found", currentUserId);
                    throw new NotFoundException("Current user not found", "CURRENT_USER_NOT_FOUND");
                }

                var isAdmin = await _userManager.IsInRoleAsync(currentUser, "Admin");

                if (!isAdmin)
                {
                    _logger.LogWarning("User with ID {CurrentUserId} is not an admin", currentUserId);
                    throw new ForbiddenException("Only admins can delete users", "ADMIN_REQUIRED");
                }

                var user = await _userManager.FindByIdAsync(id);

                if (user == null)
                {
                    _logger.LogWarning("User with ID {UserId} not found", id);
                    throw new NotFoundException($"User with ID {id} not found", "USER_NOT_FOUND");
                }

                var result = await _userManager.DeleteAsync(user);

                if (!result.Succeeded)
                {
                    var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                    _logger.LogWarning("Failed to delete user {UserId}. Errors: {Errors}", id, errors);
                    throw new IdentityException("User deletion failed", "USER_DELETION_FAILED", result.Errors.ToString());
                }

                var userDto = _mapper.Map<UserResponseDto>(user);
                var roles = await _userManager.GetRolesAsync(user);
                userDto.Role = roles.FirstOrDefault();

                _logger.LogInformation("User {UserId} deleted successfully", id);
                
                return userDto;
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