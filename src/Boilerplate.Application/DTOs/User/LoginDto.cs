using System.ComponentModel.DataAnnotations;

namespace Boilerplate.Application.DTOs.User
{
    public class LoginDto
    {
        [Required(AllowEmptyStrings = false, ErrorMessage = "Email or Mobile Number is required.")] 
        public string EmailOrMobilePhone { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Password is required.")]
        public string Password { get; set; }
    }

    public class CheckEmail
    {
        [Required(AllowEmptyStrings = false, ErrorMessage = "Email or Mobile Number is required.")] 
        public string EmailOrMobilePhone { get; set; } 
    }
}
