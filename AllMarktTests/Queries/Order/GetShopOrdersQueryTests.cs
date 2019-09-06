using AllMarkt.Entities;
using AllMarkt.Queries.Orders;
using AllMarkt.ViewModels;
using FluentAssertions;
using MediatR;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace AllMarktTests.Queries.Order
{
    public class GetShopOrdersQueryTests : AllMarktContextTests
    {
        private readonly IRequestHandler<GetShopOrdersQuery, IEnumerable<OrderViewModel>> _getShopOrdersQueryHandler;

        public GetShopOrdersQueryTests()
        {
            _getShopOrdersQueryHandler = new GetShopOrdersQueryHandler(AllMarktQueryContextIM);
        }

        [Fact]
        public async Task GetShopOrdersQueryHandle_Returns_Empty()
        {
            //Arrange
            int shopId = 1;

            //Act
            var result = await _getShopOrdersQueryHandler
                .Handle(new GetShopOrdersQuery { ShopId = shopId }, CancellationToken.None);

            //Assert
            result.Should().BeEmpty();
        }

        [Fact]
        public async Task GetShopOrdersQueryHandle_Returns_ExistingOrders()
        {
            //Arrange
            AllMarkt.Entities.Shop seller = new AllMarkt.Entities.Shop
            {
                Address = "aaaaa",
                Comments = null,
                CUI = "aaaaddd",
                IBAN = "aaaaa",
                Orders = null,
                PhoneNumber = "0123654789",
                ProductCategories = null,
                ShopCategoryLink = null,
                SocialCapital = 4
            };
            AllMarktContextIM.Shops.Add(seller);
            await AllMarktContextIM.SaveChangesAsync();

            AllMarkt.Entities.Order order = new AllMarkt.Entities.Order()
            {
                Seller = seller
            };
            AllMarktContextIM.Orders.Add(order);
            await AllMarktContextIM.SaveChangesAsync();

            //Act
            var result = await _getShopOrdersQueryHandler
                .Handle(new GetShopOrdersQuery() { ShopId = seller.Id }, CancellationToken.None);

            //Assert
            result.Count().Should().Be(1);

        }

    }
}
