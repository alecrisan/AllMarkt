using AllMarkt.Commands.Category;
using AllMarkt.Queries.Category;
using AllMarkt.Services.Interfaces;
using AllMarkt.ViewModels;
using MediatR;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AllMarkt.Services
{
    public class CategoriesService : ICategoriesService
    {
        private readonly IMediator _mediator;

        public CategoriesService(IMediator mediator)
        {
            _mediator = mediator;
        }

        public Task<IEnumerable<CategoryViewModel>> GetAllAsync()
            => _mediator.Send(new GetAllCategoriesQuery());

        public Task SaveAsync(CategoryViewModel viewModel)
            => viewModel.Id == 0 ? AddCategory(viewModel) : EditCategory(viewModel);

        private Task AddCategory(CategoryViewModel viewModel)
            => _mediator.Send(new AddCategoryCommand
            {
                Name = viewModel.Name,
                Description = viewModel.Description
            });

        private Task EditCategory(CategoryViewModel viewModel)
            => _mediator.Send(new EditCategoryCommand
            {
                Id = viewModel.Id,
                Name = viewModel.Name,
                Description = viewModel.Description
            });

        public Task DeleteAsync(int id)
            => _mediator.Send(new DeleteCategoryCommand
            {
                Id = id
            });
    }
}
