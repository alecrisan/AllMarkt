using System.ComponentModel.DataAnnotations;

namespace AllMarkt.ViewModels
{
    public class OrderItemViewModel
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public int Amount { get; set; }

        [Required]
        public float Price { get; set; }
    }
}
