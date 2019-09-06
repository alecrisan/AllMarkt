using AllMarkt.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace AllMarkt.ViewModels
{
    public class OrderViewModel
    {
        public int Id { get; set; }

        public int ShopId { get; set; }

        [Required,MaxLength(50)]
        public string ShopName { get; set; }

        public int CustomerId { get; set; }
        
        [Required,MaxLength(50)]
        public string CustomerName { get; set; }

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

        public ICollection<OrderItemViewModel> OrderItems { get; set; }


    }
}
