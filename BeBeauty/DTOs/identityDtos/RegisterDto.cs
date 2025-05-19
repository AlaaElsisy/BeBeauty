using System.ComponentModel.DataAnnotations;

namespace BeBeauty.DTOs.identity
{
    public class RegisterDto
    {
        [Required]
        public string UserName { get; set; }
       


        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Passwords do not match.")]
        public string ConfirmPassword { get; set; }

        [Phone]
        public string PhoneNumber { get; set; }


    }
}
