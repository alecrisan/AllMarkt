using AllMarkt.ViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AllMarkt.Services.Interfaces
{
    public interface IOrdersService
    {
        Task<IEnumerable<OrderViewModel>> GetAllAsync();

        Task<IEnumerable<OrderViewModel>> GetShopOrdersAsync(int shopId);

        Task<IEnumerable<OrderViewModel>> GetCustomerOrdersAsync(int customerId);

        Task SaveAsync(OrderViewModel viewModel);

        Task DeleteAsync(int Id);
    }
}
