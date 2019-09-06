using AllMarkt.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AllMarkt.Services.Interfaces
{
    public interface IProductCategoriesService
    {
        Task<IEnumerable<ProductCategoryViewModel>> GetAllByShopIdAsync(int id);
        Task<IEnumerable<ProductCategoryViewModel>> GetAllAsync();
        Task SaveAsync(ProductCategoryViewModel productCategoryViewModel);
        Task DeleteAsync(int id);
            
    }
}
