using AllMarkt.ViewModels;
using System.Threading.Tasks;

namespace AllMarkt.Services.Interfaces
{
    public interface IShopCategoryService
    {
        Task SaveAsync(ShopCategoryViewModel viewModel);
        Task DeleteAsync(ShopCategoryViewModel viewModel);
    }
}
