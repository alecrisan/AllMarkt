using AllMarkt.ViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AllMarkt.Services.Interfaces
{
    public interface IProductCommentsService
    {
        Task<IEnumerable<ProductCommentViewModel>> GetAllAsync();

        Task SaveAsync(ProductCommentViewModel viewModel);

        Task DeleteAsync(int id);

        Task<IEnumerable<ProductCommentViewModel>> GetProductCommentsByProductIdAsync(int id);
    }
}
