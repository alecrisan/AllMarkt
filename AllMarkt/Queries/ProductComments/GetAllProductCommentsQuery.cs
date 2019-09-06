using AllMarkt.Data;
using AllMarkt.ViewModels;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace AllMarkt.Queries.ProductComments
{
    public class GetAllProductCommentsQuery : IRequest<IEnumerable<ProductCommentViewModel>>
    {
    }

    public class GetAllProductCommentsQueryHandler : IRequestHandler<GetAllProductCommentsQuery, IEnumerable<ProductCommentViewModel>>
    {
        private readonly AllMarktQueryContext _allMarktQueryContext;

        public GetAllProductCommentsQueryHandler(AllMarktQueryContext allMarktQueryContext)
        {
            _allMarktQueryContext = allMarktQueryContext;
        }

        public async Task<IEnumerable<ProductCommentViewModel>> Handle(GetAllProductCommentsQuery request, CancellationToken cancellationToken)
        {
            var productComments = await _allMarktQueryContext
                .ProductComments
                .ToListAsync(cancellationToken);

            return from productComment in productComments
                   select new ProductCommentViewModel
                   {
                       Id = productComment.Id,
                       Rating = productComment.Rating,
                       Text = productComment.Text,
                       AddedById = productComment.AddedBy.Id,
                       AddedByName = productComment.AddedBy.DisplayName,
                       ProductId = productComment.Product.Id,
                       ProductName = productComment.Product.Name,
                       DateSent = productComment.DateAdded
                   };
        }
    }
}
