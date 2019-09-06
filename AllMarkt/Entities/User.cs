using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace AllMarkt.Entities
{
    public class User
    {
        [Key]
        public int Id { get; set; }

        [EmailAddress, Required, MaxLength(80)]
        public string Email { get; set; }

        [Required, MaxLength(64)]
        public string Password { get; set; }

        [Required, MaxLength(50)]
        public string DisplayName { get; set; }

        [EnumDataType(typeof(UserRole))]
        public UserRole UserRole { get; set; }

        public ICollection<PrivateMessage> ReceivedMessages { get; set; }

        public ICollection<PrivateMessage> SentMessages { get; set; }

        [DefaultValue("true")]
        public bool IsEnabled { get; set; }
    }
}
