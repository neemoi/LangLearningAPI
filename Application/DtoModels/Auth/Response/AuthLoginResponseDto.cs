namespace Application.DtoModels.Auth.Response
{
    public class AuthLoginResponseDto
    {
        public string? Id { get; set; }

        public string? Email { get; set; }

        public string? Username { get; set; }

        public bool IsBlocked { get; set; }

        public string? Role { get; set; }

        public string? Token { get; set; }
    }
}
