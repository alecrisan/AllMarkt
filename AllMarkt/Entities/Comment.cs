using System;
using System.ComponentModel.DataAnnotations;

namespace AllMarkt.Entities
{
    public class Comment
    {
        public int Id { get; set; }

        [Required, Range(1, 5)]
        public int Rating { get; set; }

        [MaxLength(1023)]
        public string Text { get; set; }

        [Required]
        public User AddedBy { get; set; }

        [Required]
        public DateTime DateAdded { get; set; }
    }
}
