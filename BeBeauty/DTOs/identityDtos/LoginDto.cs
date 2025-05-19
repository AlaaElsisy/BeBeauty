using System.ComponentModel.DataAnnotations;

namespace BeBeauty.DTOs.identity
{
    public class LoginDto
    {
        [Required]

        public string UserName { get; set; }
        
        //public string Email { get; set; }
        public string Password { get; set; }
    }
}
