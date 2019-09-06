using AllMarkt.Data;
using AllMarkt.ViewModels;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;

namespace AllMarkt.Queries.User.Customer
{
    public class GetShopByIdQuery : IRequest<ShopViewModel>
    {
        public int Id { get; set; }
    }

    public class GetShopByIdQueryHandler : IRequestHandler<GetShopByIdQuery, ShopViewModel>
    {
        private readonly AllMarktQueryContext _allMarktQueryContext;

        public GetShopByIdQueryHandler(AllMarktQueryContext allMarktQueryContext)
        {
            _allMarktQueryContext = allMarktQueryContext;
        }

        public async Task<ShopViewModel> Handle(GetShopByIdQuery request, CancellationToken cancellationToken)
        {
            var shopById = await _allMarktQueryContext
                .Shops
                .FirstOrDefaultAsync(s => s.Id == request.Id);

            if (shopById != null)
            {
                return new ShopViewModel
                {
                    Id = shopById.Id,
                    UserDisplayName = shopById.User.DisplayName,
                    Address = shopById.Address,
                    PhoneNumber = shopById.PhoneNumber,
                    UserId = shopById.UserId,
                    IBAN = shopById.IBAN,
                    CUI  = shopById.CUI,
                    SocialCapital = shopById.SocialCapital,
                };
            }
            return null;
        }

    }
}
