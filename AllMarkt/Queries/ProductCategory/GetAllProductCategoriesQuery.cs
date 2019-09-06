using AllMarkt.Data;
using AllMarkt.ViewModels;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace AllMarkt.Queries.ProductCategory
{
    public class GetAllProductCategoriesQuery : IRequest<IEnumerable<ProductCategoryViewModel>>
    { 
    }

    public class GetAllProductCategoriesQueryHandler : IRequestHandler<GetAllProductCategoriesQuery, IEnumerable<ProductCategoryViewModel>>
    {
        private readonly AllMarktQueryContext _allMarktQueryContext;

        public GetAllProductCategoriesQueryHandler(AllMarktQueryContext allMarktQueryContext)
        {
            _allMarktQueryContext = allMarktQueryContext;
        }

        public async Task<IEnumerable<ProductCategoryViewModel>> Handle(GetAllProductCategoriesQuery request, CancellationToken cancellationToken)
        {
            var productCategories = await _allMarktQueryContext
                .ProductCategories
                .ToListAsync(cancellationToken);

            return from productCategory in productCategories
                   select new ProductCategoryViewModel
                   {
                       Id = productCategory.Id,
                       Name = productCategory.Name,
                       Description = productCategory.Description,
                       ShopId = productCategory.Shop.Id,
                       ShopName = productCategory.Shop.User.DisplayName
                   };
                
        }
    }
}
