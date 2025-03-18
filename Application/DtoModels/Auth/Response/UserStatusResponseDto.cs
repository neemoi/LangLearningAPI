namespace Application.DtoModels.Auth.Response
{
    public class UserStatusResponseDto
    {
        public string? Id { get; set; }
        
        public string? Email { get; set; }
        
        public string? Username { get; set; }
        
        public bool IsBlocked { get; set; }
    }
}