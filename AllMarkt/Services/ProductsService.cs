using AllMarkt.Commands.Product;
using AllMarkt.Queries.Product;
using AllMarkt.Services.Interfaces;
using AllMarkt.ViewModels;
using MediatR;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AllMarkt.Services
{
    public class ProductsService : IProductsService
    {
        private readonly IMediator _mediator;

        public ProductsService(IMediator mediator)
        {
            _mediator = mediator;
        }

        public Task<IEnumerable<ProductViewModel>> GetAllAsync()
            => _mediator.Send(new GetAllProductsQuery());

        public Task<IEnumerable<ProductViewModel>> GetAllByProductCategoryAsync(int id)
            => _mediator.Send(new GetAllProductsByProductCategoryQuery(id));

        public Task SaveAsync(ProductViewModel viewModel)
             => viewModel.Id == 0 ? AddAsync(viewModel) : EditAsync(viewModel);

        public Task AddAsync(ProductViewModel viewModel)
            => _mediator.Send(new AddProductCommand
            {
                Name = viewModel.Name,
                Description = viewModel.Description,
                Price = viewModel.Price,
                ImageURI = viewModel.ImageURI,
                State = viewModel.State,
                ProductCategoryId = viewModel.ProductCategoryId
            });

        public Task EditAsync(ProductViewModel viewModel)
            => _mediator.Send(new EditProductCommand
            {
                Id = viewModel.Id,
                Name = viewModel.Name,
                Description = viewModel.Description,
                Price = viewModel.Price,
                ImageURI = viewModel.ImageURI,
                State = viewModel.State
            });

        public Task DeleteAsync(int id)
            => _mediator.Send(new DeleteProductCommand
            {
                Id = id
            });

        public Task<IEnumerable<ProductWithRatingViewModel>> GetProductWithRatingByCategoryAsync(int id)
             => _mediator.Send(new GetProductsWithRatingByCategoryIdQuery
             {
                 ProductCategoryId = id
             });

        public Task<IEnumerable<ProductWithRatingViewModel>> GetAllProductsByShopAsync(int id)
        => _mediator.Send(new GetAllProductsByShopQuery
        {
            ShopId = id
        });
    }
}
