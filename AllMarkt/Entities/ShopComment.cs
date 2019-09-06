using System.ComponentModel.DataAnnotations;

namespace AllMarkt.Entities
{
    public class ShopComment : Comment
    {
        [Required]
        public Shop Shop { get; set; }
    }
}
