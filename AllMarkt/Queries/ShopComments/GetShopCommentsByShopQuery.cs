using AllMarkt.Data;
using AllMarkt.ViewModels;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace AllMarkt.Queries.ShopComments
{
    public class GetShopCommentsByShopQuery : IRequest<IEnumerable<ShopCommentViewModel>>
    {
        public int Id;

        public GetShopCommentsByShopQuery(int id)
        {
            this.Id = id;
        }
    }

    public class GetShopCommentsByShopQueryHandler : IRequestHandler<GetShopCommentsByShopQuery, IEnumerable<ShopCommentViewModel>>
    {
        private readonly AllMarktQueryContext _allMarktQueryContext;

        public GetShopCommentsByShopQueryHandler(AllMarktQueryContext allMarktQueryContext)
        {
            _allMarktQueryContext = allMarktQueryContext;
        }

        public async Task<IEnumerable<ShopCommentViewModel>> Handle(GetShopCommentsByShopQuery request, CancellationToken cancellationToken)
        {
            var shopComments = await _allMarktQueryContext
                .ShopComments
                .Where(x => x.Shop.Id == request.Id)
                .ToListAsync(cancellationToken);

            return from shopComment in shopComments
                   select new ShopCommentViewModel
                   {
                       Id = shopComment.Id,
                       Rating = shopComment.Rating,
                       Text = shopComment.Text,
                       AddedById = shopComment.AddedBy.Id,
                       AddedByName = shopComment.AddedBy.DisplayName,
                       ShopId = shopComment.Shop.Id,
                       ShopName = shopComment.Shop.User.DisplayName
                   };
        }
    }
}
