using AllMarkt.Data;
using AllMarkt.ViewModels;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace AllMarkt.Queries.Product
{
    public class GetAllProductsQuery : IRequest<IEnumerable<ProductViewModel>>
    {
    }

    public class GetAllProductsQueryHandler : IRequestHandler<GetAllProductsQuery, IEnumerable<ProductViewModel>>
    {
        private readonly AllMarktQueryContext _allMarktQueryContext;

        public GetAllProductsQueryHandler(AllMarktQueryContext allMarktQueryContext)
        {
            _allMarktQueryContext = allMarktQueryContext;
        }

        public async Task<IEnumerable<ProductViewModel>> Handle(GetAllProductsQuery request, CancellationToken cancellationToken)
        {
            var products = await _allMarktQueryContext
                .Products
                .ToListAsync(cancellationToken);

            return from product in products
                   select new ProductViewModel
                   {
                       Id = product.Id,
                       Name = product.Name,
                       Description = product.Description,
                       Price = product.Price,
                       State = product.State,
                       ProductCategoryId = product.ProductCategory.Id,
                       ProductCategoryName = product.ProductCategory.Name
                   };
        }
    }
}
