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
    public class GetAllShopCommentsQuery : IRequest<IEnumerable<ShopCommentViewModel>>
    {
    }

    public class GetAllShopCommentsQueryHandler : IRequestHandler<GetAllShopCommentsQuery, IEnumerable<ShopCommentViewModel>>
    {
        private readonly AllMarktQueryContext _allMarktQueryContext;

        public GetAllShopCommentsQueryHandler(AllMarktQueryContext allMarktQueryContext)
        {
            _allMarktQueryContext = allMarktQueryContext;
        }

        public async Task<IEnumerable<ShopCommentViewModel>> Handle(GetAllShopCommentsQuery request, CancellationToken cancellationToken)
        {
            var shopComments = await _allMarktQueryContext
                .ShopComments
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
