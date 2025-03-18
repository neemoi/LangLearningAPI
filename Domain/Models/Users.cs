using Microsoft.AspNetCore.Identity;

namespace Domain.Models
{
    public class Users : IdentityUser
    {
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