using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace AllMarkt.Entities
{
    public class Shop
    {
        public int Id { get; set; }

        public int UserId { get; set; }

        public User User { get; set; }

        [RegularExpression("^0[0-9]{9}$")]
        public string PhoneNumber { get; set; }

        [RegularExpression("^RO[0-9]{1,9}[0-9a-zA-Z]{1}$")]
        public string CUI { get; set; }

        [StringLength(24)]
        public string IBAN { get; set; }

        [MaxLength(255)]
        public string Address { get; set; }

        public double SocialCapital { get; set; }

        public ICollection<ShopCategory> ShopCategoryLink { get; set; }

        public ICollection<ProductCategory> ProductCategories { get; set; }

        public ICollection<ShopComment> Comments { get; set; }

        public ICollection<Order> Orders { get; set; }
    }
}
