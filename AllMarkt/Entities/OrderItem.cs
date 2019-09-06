using System.ComponentModel.DataAnnotations;

namespace AllMarkt.Entities
{
    public class OrderItem
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public int Amount { get; set; }

        [Required]
        public float Price { get; set; }

        public Order Order { get; set; }
    }
}
