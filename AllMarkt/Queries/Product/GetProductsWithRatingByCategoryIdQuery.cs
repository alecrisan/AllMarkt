using AllMarkt.Data;
using AllMarkt.ViewModels;
using MediatR;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading;
using System.Threading.Tasks;

namespace AllMarkt.Queries.Product
{
    public class GetProductsWithRatingByCategoryIdQuery : IRequest<IEnumerable<ProductWithRatingViewModel>>
    {
        public int ProductCategoryId { get; set; }
    }

    public class GetProductsWithRatingByCategoryIdQueryHandler : IRequestHandler<GetProductsWithRatingByCategoryIdQuery, IEnumerable<ProductWithRatingViewModel>>
    {
        private readonly IAllMarktQueryContext _allMarktQueryContext;

        public GetProductsWithRatingByCategoryIdQueryHandler(IAllMarktQueryContext allMarktQueryContext)
        {
            _allMarktQueryContext = allMarktQueryContext;
        }

        public Task<IEnumerable<ProductWithRatingViewModel>> Handle(GetProductsWithRatingByCategoryIdQuery request, CancellationToken cancellationToken)
            => _allMarktQueryContext.ExecuteStoredProcedureAsync<ProductWithRatingViewModel>(
                "GetProductsByCategoryIdWithAverageRating",
                new SqlParameter("@productCategoryId", request.ProductCategoryId));
    }
}
