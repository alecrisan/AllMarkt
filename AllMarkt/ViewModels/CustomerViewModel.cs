using System.ComponentModel.DataAnnotations;

namespace AllMarkt.ViewModels
{
    public class CustomerViewModel
    {
        public int Id { get; set; }

        public int UserId { get; set; }

        public string UserDisplayName { get; set; }

        [RegularExpression("^0[0-9]{9}$")]
        public string PhoneNumber { get; set; }

        [MaxLength(255)]
        public string Address { get; set; }

    }
}
