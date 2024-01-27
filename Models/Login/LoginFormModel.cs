using System.ComponentModel.DataAnnotations;

namespace Authentication.System.API.Models.Login
{
    public class LoginFormModel
    {
        [Required]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
