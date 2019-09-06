using AllMarkt.ViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AllMarkt.Services.Interfaces
{
    public interface IShopCommentsService
    {
        Task<IEnumerable<ShopCommentViewModel>> GetAllAsync();

        Task SaveAsync(ShopCommentViewModel viewModel);

        Task DeleteAsync(int id);

        Task<IEnumerable<ShopCommentViewModel>> GetShopCommentsByShopId(int id);
    }
}
