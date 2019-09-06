using System.ComponentModel.DataAnnotations.Schema;

namespace AllMarkt.Entities
{
    public class Admin
    {
        public int Id { get; set; }

        public int UserId { get; set; }

        public User User { get; set; }
    }
}
