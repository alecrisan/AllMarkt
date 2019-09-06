using AllMarkt.Data;
using AllMarkt.ViewModels;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace AllMarkt.Queries.User.Shop
{
    public class GetShopsByCategoryIdQuery : IRequest<IEnumerable<ShopViewModel>>
    {
        public int Id { get; set; }
    }

    public class GetShopsByCategoryIdQueryHandler : IRequestHandler<GetShopsByCategoryIdQuery, IEnumerable<ShopViewModel>>
    {
        private readonly AllMarktQueryContext _allMarktQueryContext;

        public GetShopsByCategoryIdQueryHandler(AllMarktQueryContext allMarktQueryContext)
        {
            _allMarktQueryContext = allMarktQueryContext;
        }

        public async Task<IEnumerable<ShopViewModel>> Handle(GetShopsByCategoryIdQuery request, CancellationToken cancellationToken)
        {
            var shops = await _allMarktQueryContext
                .Shops
                .Include(s => s.ShopCategoryLink)
                .ThenInclude(sc => sc.Category)
                .ToListAsync();

            List<ShopViewModel> shopList = new List<ShopViewModel>();

            foreach (var shop in shops)
            {
                foreach (var category in shop.ShopCategoryLink)
                {
                    if (category.CategoryId == request.Id)
                    {
                        shopList.Add(new ShopViewModel
                        {
                            Address = shop.Address,
                            CUI = shop.CUI,
                            IBAN = shop.IBAN,
                            Id = shop.Id,
                            PhoneNumber = shop.PhoneNumber,
                            SocialCapital = shop.SocialCapital,
                            UserDisplayName = shop.User.DisplayName,
                            UserId = shop.UserId
                        });
                    }
                }
            }
            return shopList;
        }
    }
}
