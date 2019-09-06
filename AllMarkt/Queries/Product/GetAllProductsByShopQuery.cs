using AllMarkt.Data;
using AllMarkt.ViewModels;
using MediatR;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading;
using System.Threading.Tasks;

namespace AllMarkt.Queries.Product
{
    public class GetAllProductsByShopQuery : IRequest<IEnumerable<ProductWithRatingViewModel>>
    {
        public int ShopId { get; set; }
    }
    public class GetAllProductsByShopQueryHandler : IRequestHandler<GetAllProductsByShopQuery, IEnumerable<ProductWithRatingViewModel>>
    {
        private readonly IAllMarktQueryContext _allmarktQueryContext;

        public GetAllProductsByShopQueryHandler(IAllMarktQueryContext allMarktQueryContext)
        {
            _allmarktQueryContext = allMarktQueryContext;
        }

        public Task<IEnumerable<ProductWithRatingViewModel>> Handle(GetAllProductsByShopQuery request, CancellationToken cancellationToken)
            => _allmarktQueryContext.ExecuteStoredProcedureAsync<ProductWithRatingViewModel>(
                "GetAllProductsByShop",
                new SqlParameter("@shopId",request.ShopId));
    }
}
