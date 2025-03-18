using Application.DtoModels.AdminUsers;
using Application.DtoModels.User;

namespace Application.Services.Interfaces.IServices
{
    public interface IUserService
    {
        Task<UserByIdDto> GetUserByIdAsync(string id);
        
        Task<IEnumerable<UserDto>> GetAllUsersAsync();

        Task<UserResponseDto> UpdateUserAsync(string id, UpdateUserDto updateUserDto);
        
        Task<UserResponseDto> DeleteUserAsync(string id, string currentUserId);
    }
}
