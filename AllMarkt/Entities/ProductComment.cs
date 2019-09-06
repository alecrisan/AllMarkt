using System.ComponentModel.DataAnnotations;

namespace AllMarkt.Entities
{
    public class ProductComment : Comment
    {
        [Required]
        public Product Product { get; set; }
    }
}
