using Microsoft.AspNetCore.Identity;

namespace BeBeauty.Models.identity
{
    public class ApplicationUser : IdentityUser
    {
        public string PhoneNumber { get; set; }
        public string FullName { get; set; }

    }
}
