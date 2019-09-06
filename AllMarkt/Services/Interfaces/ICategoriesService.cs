using AllMarkt.ViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AllMarkt.Services.Interfaces
{
    public interface ICategoriesService
    {
        Task<IEnumerable<CategoryViewModel>> GetAllAsync();
        Task SaveAsync(CategoryViewModel viewModel);
        Task DeleteAsync(int id);
    }
}
