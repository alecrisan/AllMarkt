using AllMarkt.Entities;
using System.ComponentModel.DataAnnotations;

namespace AllMarkt.ViewModels
{
    public class UserEditViewModel
    {
        public int Id { get; set; }

        [EmailAddress, MaxLength(80)]
        public string Email { get; set; }

        [MaxLength(64)]
        public string OldPassword { get; set; }

        [MaxLength(64)]
        public string NewPassword { get; set; }

        [MaxLength(50)]
        public string DisplayName { get; set; }

        [EnumDataType(typeof(UserRole))]
        public string UserRole { get; set; }
    }
}
