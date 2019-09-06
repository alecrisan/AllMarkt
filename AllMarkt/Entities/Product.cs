using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace AllMarkt.Entities
{
    public class Product
    {
        public int Id { get; set; }

        [Required, MaxLength(50)]
        public string Name { get; set; }

        [Required, MaxLength(255)]
        public string Description { get; set; }

        [Required]
        public double Price { get; set; }

        public string ImageURI { get; set; }

        [DefaultValue("true")]
        public bool State { get; set; }

        public ProductCategory ProductCategory { get; set; }

        public ICollection<ProductComment> Comments { get; set; }
    }
}
