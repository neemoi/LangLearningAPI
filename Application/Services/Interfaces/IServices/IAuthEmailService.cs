namespace Application.Services.Interfaces.IServices.Auth
{
    public interface IAuthEmailService
    {
        Task SendEmailAsync(string toEmail, string subject, string body);
    }
}
