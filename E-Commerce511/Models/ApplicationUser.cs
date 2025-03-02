using Microsoft.AspNetCore.Identity;

namespace E_Commerce511.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string? Address { get; set; }
    }
}
