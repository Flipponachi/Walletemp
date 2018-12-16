using System.ComponentModel.DataAnnotations;

namespace Walletemp.Dto
{
    public class RegisterDto
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }

        [Compare("Password")]
        public string ComparePassword { get; set; }
    }
}