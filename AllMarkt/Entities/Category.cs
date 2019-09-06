using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace AllMarkt.Entities
{
    public class Category
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(80)]
        public string Name { get; set; }

        [MaxLength(255)]
        public string Description { get; set; }

        public ICollection<ShopCategory> ShopCategoryLink { get; set; }
    }
}
