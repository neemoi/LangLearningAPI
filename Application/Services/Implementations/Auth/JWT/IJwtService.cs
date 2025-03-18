using Domain.Models;

namespace Application.Services.Implementations.Auth.JWT
{
    public interface IJwtService
    {
        Task<string> GenerateTokenAsync(Users user);
    }
}