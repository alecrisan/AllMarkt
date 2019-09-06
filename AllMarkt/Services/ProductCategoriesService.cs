using AllMarkt.Commands.ProductCategory;
using AllMarkt.Queries.ProductCategory;
using AllMarkt.Services.Interfaces;
using AllMarkt.ViewModels;
using MediatR;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AllMarkt.Services
{
    public class ProductCategoriesService : IProductCategoriesService
    {
        private readonly IMediator _mediator;

        public ProductCategoriesService(IMediator mediator)
        {
            _mediator = mediator;
        }


        public Task<IEnumerable<ProductCategoryViewModel>> GetAllAsync()
            => _mediator.Send(new GetAllProductCategoriesQuery());
        public Task<IEnumerable<ProductCategoryViewModel>> GetAllByShopIdAsync(int id)
        => _mediator.Send(new GetAllProductCategoriesByShopQuery(id));

        public Task SaveAsync(ProductCategoryViewModel productCategoryViewModel)
            => productCategoryViewModel.Id == 0 ? 
            AddProductCategory(productCategoryViewModel) 
            : EditProductCategory(productCategoryViewModel);

        private Task AddProductCategory(ProductCategoryViewModel productCategoryViewModel)
            => _mediator.Send(new AddProductCategoryCommand
            {
                Name = productCategoryViewModel.Name,
                Description = productCategoryViewModel.Description,
                ShopId = productCategoryViewModel.ShopId
            });

        private Task EditProductCategory(ProductCategoryViewModel productCategoryViewModel)
            => _mediator.Send(new EditProductCategoryCommand
            {
                Name = productCategoryViewModel.Name,
                Description = productCategoryViewModel.Description,
                Id = productCategoryViewModel.Id
            });

        public async Task DeleteAsync(int id)
            => await _mediator.Send(new DeleteProductCategoryCommand
            {
                Id = id
            });
    }
}
