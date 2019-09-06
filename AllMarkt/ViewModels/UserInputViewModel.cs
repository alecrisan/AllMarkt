using System.ComponentModel.DataAnnotations;

namespace AllMarkt.ViewModels
{
    public class UserInputViewModel
    {
        public int Id { get; set; }

        [EmailAddress, Required, MaxLength(80)]
        public string Email { get; set; }

        [Required, MaxLength(64)]
        public string Password { get; set; }

        [Required, MaxLength(50)]
        public string DisplayName { get; set; }

        public string UserRole { get; set; }

    }
}
