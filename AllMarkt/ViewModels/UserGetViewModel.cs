using System;
using System.ComponentModel.DataAnnotations;
using AllMarkt.Entities;

namespace AllMarkt.ViewModels
{
    public class UserGetViewModel
    {
        public int Id { get; set; }

        [EmailAddress, Required, MaxLength(80)]
        public string Email { get; set; }

        [Required, MaxLength(64)]
        public string Password { get; set; }

        [Required, MaxLength(50)]
        public string DisplayName { get; set; }

        [EnumDataType(typeof(UserRole))]
        public string UserRole { get; set; }

        public string Token { get; set; }

        public bool IsEnabled { get; set; }
    }
}

