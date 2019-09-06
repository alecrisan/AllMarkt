using System.ComponentModel.DataAnnotations;

namespace AllMarkt.ViewModels
{
    public class UserLoginResponseViewModel
    {
        [Required]
        public string Token { get; set; }
        [Required, EmailAddress]
        public string Email { get; set; }
        public string DisplayName { get; set; }
        [Required]
        public string UserRole { get; set; }
        public bool IsEnabled { get; set; }
    }
}
