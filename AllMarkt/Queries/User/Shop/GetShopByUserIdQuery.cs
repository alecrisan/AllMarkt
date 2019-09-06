using AllMarkt.Data;
using AllMarkt.ViewModels;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace AllMarkt.Queries.User.Customer
{
    public class GetShopByUserIdQuery : IRequest<ShopViewModel>
    {
        public int UserId { get; set; }
    }

    public class GetShopByUserIdQueryHandler : IRequestHandler<GetShopByUserIdQuery, ShopViewModel>
    {
        private readonly AllMarktQueryContext _allMarktQueryContext;

        public GetShopByUserIdQueryHandler(AllMarktQueryContext allMarktQueryContext)
        {
            _allMarktQueryContext = allMarktQueryContext;
        }

        public async Task<ShopViewModel> Handle(GetShopByUserIdQuery request, CancellationToken cancellationToken)
        {
            var shopById = await _allMarktQueryContext
                .Shops
                .FirstOrDefaultAsync(u => u.UserId == request.UserId);

            if (shopById != null)
            {
                return new ShopViewModel
                {
                    Id = shopById.Id,
                    UserDisplayName = shopById.User.DisplayName,
                    Address = shopById.Address,
                    PhoneNumber = shopById.PhoneNumber,
                    UserId = shopById.UserId,
                    CUI = shopById.CUI,
                    SocialCapital = shopById.SocialCapital,
                    IBAN = shopById.IBAN
                };
            }
            return null;
        }

    }
}
