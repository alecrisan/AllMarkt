using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace AllMarkt.Entities
{
    public class Order
    {
        public int Id { get; set; }

        public Shop Seller { get; set; }

        public Customer Buyer { get; set; }

        [RegularExpression("^0[0-9]{9}$")]
        public string DeliveryPhoneNumber { get; set; }

        [Required, MaxLength(255)]
        public string DeliveryAddress { get; set; }

        public float TotalPrice { get; set; }

        public DateTime TimeOfOrder { get; set; }

        [MaxLength(255)]
        public string AdditionalNotes { get; set; }

        [EnumDataType(typeof(Status))]
        public Status OrderStatus { get; set; }

        [Required]
        public string AWB { get; set; }

        public ICollection<OrderItem> OrderItems { get; set; }
    }
}
