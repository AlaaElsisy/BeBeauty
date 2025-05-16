using Microsoft.AspNetCore.Identity;

namespace BeBeauty.Models
{
    public class ApplicationUser:IdentityUser
    {
        public string FullName { get; set; }
    }
}
