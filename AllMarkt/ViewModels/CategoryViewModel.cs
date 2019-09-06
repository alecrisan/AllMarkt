using System.ComponentModel.DataAnnotations;

namespace AllMarkt.ViewModels
{
    public class CategoryViewModel
    {
        public int Id { get; set; }

        [Required, MaxLength(80)]
        public string Name { get; set; }

        [MaxLength(255)]
        public string Description { get; set; }
    }
}
