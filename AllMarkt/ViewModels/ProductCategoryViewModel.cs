using System.ComponentModel.DataAnnotations;

namespace AllMarkt.ViewModels
{
    public class ProductCategoryViewModel
    {
        public int Id { get; set; }

        [Required, MaxLength(50)]
        public string Name { get; set; }

        [MaxLength(255)]
        public string Description { get; set; }

        public int ShopId { get; set; }

        public string ShopName { get; set; }
    }
}
