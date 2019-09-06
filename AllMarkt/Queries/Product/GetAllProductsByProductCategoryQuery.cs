using AllMarkt.Data;
using AllMarkt.ViewModels;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace AllMarkt.Queries.Product
{
    public class GetAllProductsByProductCategoryQuery : IRequest<IEnumerable<ProductViewModel>>
    {
        public int ProductCategoryId { get; set; }

        public GetAllProductsByProductCategoryQuery(int id)
        {
            ProductCategoryId = id;
        }
    }

    public class GetAllProductsByProductCategoryQueryHandler : IRequestHandler<GetAllProductsByProductCategoryQuery, IEnumerable<ProductViewModel>>
    {
        private readonly AllMarktQueryContext _allMarktQueryContext;

        public GetAllProductsByProductCategoryQueryHandler(AllMarktQueryContext allMarktQueryContext)
        {
            _allMarktQueryContext = allMarktQueryContext;
        }

        public async Task<IEnumerable<ProductViewModel>> Handle(GetAllProductsByProductCategoryQuery request, CancellationToken cancellationToken)
        {
               
            var products = await _allMarktQueryContext
                .Products
                .Where(x => x.ProductCategory.Id == request.ProductCategoryId)
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
