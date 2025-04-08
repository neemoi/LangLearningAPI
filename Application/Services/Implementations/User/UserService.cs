using Application.DtoModels.AdminUsers;
using Application.DtoModels.User;
using Application.Services.Interfaces.IServices.Profile;
using Application.UnitOfWork;
using AutoMapper;
using Domain.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace Application.Services.Implementations
{
    public class UserService : IUserService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger<UserService> _logger;
        private readonly UserManager<Users> _userManager;

        public UserService(IUnitOfWork unitOfWork, IMapper mapper, ILogger<UserService> logger, UserManager<Users> userManager)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _userManager = userManager;
        }

        public async Task<UserByIdDto> GetUserByIdAsync(string id)
        {
            try
            {
                _logger.LogInformation($"Fetching user by ID: {id}");
                
                var user = await _unitOfWork.UserRepository.GetUserByIdAsync(id);
               
                if (user == null)
                {
                    _logger.LogWarning($"User with ID {id} not found.");
                    
                    return null;
                }

                var userDto = _mapper.Map<UserByIdDto>(user);

                return userDto;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error fetching user by ID: {id}");
                throw;
            }
        }

        public async Task<IEnumerable<UserDto>> GetAllUsersAsync()
        {
            try
            {
                _logger.LogInformation("Fetching all users.");
                
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
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching all users.");
                throw;
            }
        }

        public async Task<UserResponseDto> UpdateUserAsync(string id, UpdateUserDto updateUserDto)
        {
            try
            {
                _logger.LogInformation($"Updating user with ID: {id}");
                
                var user = await _unitOfWork.UserRepository.GetUserByIdAsync(id);
                
                if (user == null)
                {
                    _logger.LogWarning($"User with ID {id} not found.");
                
                    throw new KeyNotFoundException($"User with ID {id} not found.");
                }

                _mapper.Map(updateUserDto, user);

                _logger.LogInformation($"Updated user data: {System.Text.Json.JsonSerializer.Serialize(user)}");

                var result = await _unitOfWork.UserRepository.UpdateUserAsync(user);
                
                if (!result.Succeeded)
                {
                    _logger.LogWarning($"Failed to update user with ID {id}: {string.Join(", ", result.Errors.Select(e => e.Description))}");
                    throw new InvalidOperationException($"Failed to update user: {string.Join(", ", result.Errors.Select(e => e.Description))}");
                }
                
                var userDto = _mapper.Map<UserResponseDto>(user);

                var roles = await _userManager.GetRolesAsync(user);
                userDto.Role = roles.FirstOrDefault();

                _logger.LogInformation($"User with ID {id} updated successfully.");

                return userDto;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error updating user with ID: {id}");
                throw;
            }
        }

        public async Task<UserResponseDto> DeleteUserAsync(string id, string currentUserId)
        {
            try
            {
                _logger.LogInformation($"Deleting user with ID: {id}");

                var currentUser = await _userManager.FindByIdAsync(currentUserId);
                
                if (currentUser == null)
                {
                    _logger.LogWarning($"Current user with ID {currentUserId} not found.");
                    throw new KeyNotFoundException("Current user not found.");
                }

                var isAdmin = await _userManager.IsInRoleAsync(currentUser, "Admin");
                
                if (!isAdmin)
                {
                    _logger.LogWarning($"User with ID {currentUserId} is not an admin.");
                    throw new UnauthorizedAccessException("Only admins can delete users.");
                }

                var user = await _userManager.FindByIdAsync(id);
                
                if (user == null)
                {
                    _logger.LogWarning($"User with ID {id} not found.");
                    throw new KeyNotFoundException($"User with ID {id} not found.");
                }

                var result = await _userManager.DeleteAsync(user);
                
                if (!result.Succeeded)
                {
                    _logger.LogWarning($"Failed to delete user with ID {id}: {string.Join(", ", result.Errors.Select(e => e.Description))}");
                    throw new InvalidOperationException($"Failed to delete user: {string.Join(", ", result.Errors.Select(e => e.Description))}");
                }

                _logger.LogInformation($"User with ID {id} deleted successfully.");

                var userDto = _mapper.Map<UserResponseDto>(user);

                var roles = await _userManager.GetRolesAsync(user);
                userDto.Role = roles.FirstOrDefault();

                return userDto;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error deleting user with ID: {id}");
                throw;
            }
        }
    }
}