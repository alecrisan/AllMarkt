using System;
using System.ComponentModel.DataAnnotations;

namespace AllMarkt.ViewModels
{
    public class ShopCommentViewModel
    {
        public int Id { get; set; }

        [Range(1,5)]
        public int Rating { get; set; }

        [MaxLength(1023)]
        public string Text { get; set; }

        public string AddedByName { get; set; }

        public int AddedById { get; set; }

        public int ShopId { get; set; }

        public string ShopName { get; set; }

        [Required]
        public DateTime DateSent { get; set; }
    }
}
