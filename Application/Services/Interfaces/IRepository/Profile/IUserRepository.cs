using Domain.Models;
using Microsoft.AspNetCore.Identity;

namespace Application.Services.Interfaces.IRepository.Profile
{
    public interface IUserRepository
    {
        Task<Users> GetUserByIdAsync(string id);

        Task<IEnumerable<Users>> GetAllUsersAsync();

        Task<IdentityResult> UpdateUserAsync(Users user);

        Task<IdentityResult> DeleteUserAsync(string id);
    }
}
