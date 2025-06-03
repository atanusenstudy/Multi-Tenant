using Microsoft.AspNetCore.Identity;

namespace Infrastructure.Identity.Models
{
    public class ApplicationUser : IdentityUser
    {
        //public string? TenantId { get; set; } // This will be used to identify the tenant of the user
        // Additional properties can be added here as needed
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public bool IsActive { get; set; }
        public string RefreshToken { get; set; } = string.Empty; // Used for JWT Refresh Token functionality
        public DateTime RefreshTokenExpiryTime { get; set; } // Used for JWT Refresh Token functionality
    }
}
