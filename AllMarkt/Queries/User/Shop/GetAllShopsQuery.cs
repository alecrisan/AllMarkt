using AllMarkt.Data;
using AllMarkt.ViewModels;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace AllMarkt.Queries.Shop
{
    public class GetAllShopsQuery : IRequest<IEnumerable<ShopViewModel>>
    {
    }

    public class GetAllShopsQueryHandler : IRequestHandler<GetAllShopsQuery, IEnumerable<ShopViewModel>>
    {
        private readonly AllMarktQueryContext _allMarktQueryContext;

        public GetAllShopsQueryHandler(AllMarktQueryContext allMarktQueryContext)
        {
            _allMarktQueryContext = allMarktQueryContext;
        }

        public async Task<IEnumerable<ShopViewModel>> Handle(GetAllShopsQuery request, CancellationToken cancellationToken)
        {
            var shops = await _allMarktQueryContext
                .Shops
                .ToListAsync(cancellationToken);

            return from shop in shops
                   select new ShopViewModel
                   {
                       Id = shop.Id,
                       UserDisplayName = shop.User.DisplayName,
                       UserId = shop.User.Id,
                       Address = shop.Address,
                       CUI = shop.CUI,
                       IBAN = shop.IBAN,
                       PhoneNumber = shop.PhoneNumber,
                       SocialCapital = shop.SocialCapital
                   };
        }
    }
}
