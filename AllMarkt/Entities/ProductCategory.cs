using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace AllMarkt.Entities
{
    public class ProductCategory
    {

        public int Id { get; set; }

        [Required, MaxLength(50)]
        public string Name { get; set; }

        [MaxLength(255)]
        public string Description { get; set; }

        public Shop Shop { get; set; }

        public ICollection<Product> Products { get; set; }
    }
}
