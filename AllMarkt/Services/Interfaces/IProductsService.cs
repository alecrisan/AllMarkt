using AllMarkt.ViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AllMarkt.Services.Interfaces
{
    public interface IProductsService
    {
        Task<IEnumerable<ProductViewModel>> GetAllAsync();
        Task<IEnumerable<ProductViewModel>> GetAllByProductCategoryAsync(int id);
        Task<IEnumerable<ProductWithRatingViewModel>> GetProductWithRatingByCategoryAsync(int id);
        Task<IEnumerable<ProductWithRatingViewModel>> GetAllProductsByShopAsync(int id);
        Task SaveAsync(ProductViewModel viewModel);
        Task AddAsync(ProductViewModel viewModel);
        Task EditAsync(ProductViewModel viewModel);
        Task DeleteAsync(int id);
    }
}
