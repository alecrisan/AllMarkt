using System.ComponentModel.DataAnnotations;

namespace AllMarkt.ViewModels
{
    public class UserLoginRequestViewModel
    {
        [EmailAddress, Required, MaxLength(80)]
        public string Email { get; set; }

        [Required, MaxLength(64), MinLength(6)]
        public string Password { get; set; }
    }
}
