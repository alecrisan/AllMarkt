using AllMarkt.Commands.ShopCategory;
using AllMarkt.Services.Interfaces;
using AllMarkt.ViewModels;
using MediatR;
using System.Threading.Tasks;

namespace AllMarkt.Services
{
    public class ShopCategoryService : IShopCategoryService
    {
        private readonly IMediator _mediator;

        public ShopCategoryService(IMediator mediator)
        {
            _mediator = mediator;
        }

        public Task DeleteAsync(ShopCategoryViewModel viewModel)
        => _mediator.Send(new DeleteShopCategoryLinkCommand
        {
            ShopId = viewModel.ShopId,
            CategoryId = viewModel.CategoryId
        });

        public Task SaveAsync(ShopCategoryViewModel viewModel)
            => _mediator.Send(new AddShopCategoryLinkCommand
            {
                ShopId = viewModel.ShopId,
                CategoryId = viewModel.CategoryId
            });
    }
}
