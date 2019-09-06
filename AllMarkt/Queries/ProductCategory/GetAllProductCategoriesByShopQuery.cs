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
    public class GetAllProductCategoriesByShopQuery : IRequest<IEnumerable<ProductCategoryViewModel>>
    {
        public int ShopId { get; set; }

        public GetAllProductCategoriesByShopQuery(int Id)
        {
            ShopId = Id;
        }
    }

    public class GetAllProductCategoriesByShopQueryHandler : IRequestHandler<GetAllProductCategoriesByShopQuery, IEnumerable<ProductCategoryViewModel>>
    {
        private readonly AllMarktQueryContext _allMarktQueryContext;

        public GetAllProductCategoriesByShopQueryHandler(AllMarktQueryContext allMarktQueryContext)
        {
            _allMarktQueryContext = allMarktQueryContext;
        }

        public async Task<IEnumerable<ProductCategoryViewModel>> Handle(GetAllProductCategoriesByShopQuery request, CancellationToken cancellationToken)
        {
            var productCategories = await _allMarktQueryContext
                .ProductCategories
                .Where(x => x.Shop.Id == request.ShopId)
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
