using AllMarkt.Entities;
using System;
using System.ComponentModel.DataAnnotations;

namespace AllMarkt.ViewModels
{
    public class PrivateMessageViewModel
    {
        public int Id { get; set; }

        [Required, MaxLength(20)]
        public string Title { get; set; }

        [Required]
        public string Text { get; set; }

        [Required]
        public DateTime DateSent { get; set; }

        public DateTime? DateRead { get; set; }

        [Required]
        public IdAndDisplayNameUserViewModel Sender { get; set; }

        [Required]
        public IdAndDisplayNameUserViewModel Receiver { get; set; }

        [EnumDataType(typeof(DeletedBy))]
        public DeletedBy? DeletedBy { get; set; }
    }
}
