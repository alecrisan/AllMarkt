using AllMarkt.Entities;
using System.ComponentModel.DataAnnotations;

namespace AllMarkt.ViewModels
{
    public class IdAndDisplayNameUserViewModel
    {
        public IdAndDisplayNameUserViewModel()
        {
        }

        public IdAndDisplayNameUserViewModel(User user)
        {
            Id = user.Id;
            DisplayName = user.DisplayName;
        }

        public int Id { get; set; }

        [Required, MaxLength(50)]
        public string DisplayName { get; set; }
    }
}
