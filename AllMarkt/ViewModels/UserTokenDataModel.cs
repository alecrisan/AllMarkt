using System.ComponentModel.DataAnnotations;

namespace AllMarkt.ViewModels
{
    public class UserTokenDataModel
    {
        public int Id { get; set; }
        public string DisplayName { get; set; }
        public string UserRole { get; set; }
        public string Email { get; set; }
    }
}
