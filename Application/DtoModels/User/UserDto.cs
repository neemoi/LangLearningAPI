namespace Application.DtoModels.AdminUsers
{
    public class UserDto
    {
        public string? Id { get; set; }
        public string? UserName { get; set; }
        public string? Email { get; set; }
        public string? Role { get; set; }

        // Main fields
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public DateTime BirthDate { get; set; }
        public string? Gender { get; set; }
        public string? City { get; set; }
        public string? Country { get; set; }

        // Additional fields
        public string? ProfilePictureUrl { get; set; }
        public string? BackgroundImageUrl { get; set; }
        public string? RelationshipStatus { get; set; }
        public string? Interests { get; set; }
        public string? FavoriteMovies { get; set; }
        public string? FavoriteShows { get; set; }
        public string? FavoriteBooks { get; set; }
        public string? FavoriteSports { get; set; }
        public string? FavoriteMusic { get; set; }
        public string? PostalAddress { get; set; }
        public string? State { get; set; }
        public string? Website { get; set; }
    }
}
