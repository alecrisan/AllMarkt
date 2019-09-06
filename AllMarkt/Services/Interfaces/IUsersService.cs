using AllMarkt.ViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AllMarkt.Services.Interfaces
{
    public interface IUsersService
    {
        Task<UserLoginResponseViewModel> Authenticate(string email, string password);
        Task RegisterAsync(UserInputViewModel userViewModel);
        Task<IEnumerable<UserGetViewModel>> GetAllAsync();
        Task<UserGetViewModel> GetByIdAsync(int id);
        Task SaveAsync(UserInputViewModel userViewModel);
        Task DeleteAsync(int id);
        Task<IEnumerable<ShopViewModel>> GetAllShopsAsync();
        Task<IEnumerable<ShopViewModel>> GetShopsByCategoryIdAsync(int id);
        Task<IEnumerable<CustomerViewModel>> GetAllCustomersAsync();
        Task EditAsync(UserEditViewModel userViewModel);
        Task<ShopViewModel> GetShopByUserIdAsync(int userId);
        Task<ShopViewModel> GetShopByIdAsync(int id);
        Task<CustomerViewModel> GetCustomerByUserIdAsync(int userId);
        Task<CustomerViewModel> GetCustomerByIdAsync(int id);
        Task EditShopAsync(ShopViewModel shopViewModel);
        Task EditCustomerAsync(CustomerViewModel customerViewModel);
        Task DisableUserByIdAsync(UserEditViewModel userEditViewModel);
    }
}
