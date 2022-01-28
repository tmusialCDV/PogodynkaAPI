using System.ComponentModel.DataAnnotations;

namespace PogodynkaAPI.Models
{
    public class RegisterUserDto
    {
        public string Email { get; set; }

        [MinLength(6)]
        public string Password { get; set; }

        [MinLength(6)]
        public string ConfirmPassword { get; set; }
    }
}
