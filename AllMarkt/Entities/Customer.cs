using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AllMarkt.Entities
{
    public class Customer
    {
        public int Id { get; set; }

        public int UserId { get; set; }

        public User User { get; set; }

        [RegularExpression("^0[0-9]{9}$")]
        public string PhoneNumber { get; set; }

        [MaxLength(255)]
        public string Address { get; set; }

        public ICollection<Order> Orders { get; set; }
    }
}
