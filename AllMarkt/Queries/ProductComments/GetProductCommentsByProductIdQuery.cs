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
    public class GetProductCommentsByProductIdQuery : IRequest<IEnumerable<ProductCommentViewModel>>
    {
        public int Id;

        public GetProductCommentsByProductIdQuery(int id)
        {
            this.Id = id;
        }
    }

    public class GetProductCommentsByProductIdQueryHandler : IRequestHandler<GetProductCommentsByProductIdQuery, IEnumerable<ProductCommentViewModel>>
    {
        private readonly AllMarktQueryContext _allMarktQueryContext;

        public GetProductCommentsByProductIdQueryHandler(AllMarktQueryContext allMarktQueryContext)
        {
            _allMarktQueryContext = allMarktQueryContext;
        }

        public async Task<IEnumerable<ProductCommentViewModel>> Handle(GetProductCommentsByProductIdQuery request, CancellationToken cancellationToken)
        {
            var productComments = await _allMarktQueryContext
                .ProductComments
                .Where(x => x.Product.Id == request.Id)   
                .OrderByDescending(x => x.DateAdded)
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
