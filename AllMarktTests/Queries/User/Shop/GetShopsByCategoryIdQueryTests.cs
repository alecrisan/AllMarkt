using AllMarkt.Entities;
using AllMarkt.Queries.User.Shop;
using AllMarkt.ViewModels;
using FluentAssertions;
using MediatR;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace AllMarktTests.Queries.User.Shop
{
    public class GetShopsByCategoryIdQueryTests : AllMarktContextTests
    {
        private readonly IRequestHandler<GetShopsByCategoryIdQuery, IEnumerable<ShopViewModel>> _getShopsByCategoryIdQueryHandler;

        public GetShopsByCategoryIdQueryTests()
        {
            _getShopsByCategoryIdQueryHandler = new GetShopsByCategoryIdQueryHandler(AllMarktQueryContextIM);
        }

        [Fact]
        public async Task GetShopsByCategoryIdQueryHandler_ReturnsEmpty()
        {
            //Arrange

            //Act
            var result = await _getShopsByCategoryIdQueryHandler.Handle(new GetShopsByCategoryIdQuery { Id = 1 }, CancellationToken.None);

            //Assert
            result.Should().BeEmpty();
        }
       
        [Fact]
        public async Task GetShopsByCategoryIdQueryHandler_Returns_ExistingShops()
        {
            //Arrange
            var user = new AllMarkt.Entities.User
            {
                Email = "Email1@yahoo.com",
                Password = "123",
                DisplayName = "name1",
                UserRole = UserRole.Shop
            };

            await AllMarktContextIM.Users.AddAsync(user);
            await AllMarktContextIM.SaveChangesAsync();

            var shop = new AllMarkt.Entities.Shop
            {
                ProductCategories = null,
                User = user,
                Address = "address",
                Comments = null,
                CUI = "RO123456789c",
                IBAN = new string('8', 24),
                PhoneNumber = "01234567891",
                SocialCapital = 23
            };

            var category = new AllMarkt.Entities.Category
            {
                Description = "Desc",
                Name = "Category1"
            };

            await AllMarktContextIM.Shops.AddAsync(shop);
            await AllMarktContextIM.Categories.AddAsync(category);
            await AllMarktContextIM.SaveChangesAsync();

            var link = new AllMarkt.Entities.ShopCategory
            {
                Shop = shop,
                ShopId = shop.Id,
                Category = category,
                CategoryId = category.Id
            };
            await AllMarktContextIM.ShopCategories.AddAsync(link);
            await AllMarktContextIM.SaveChangesAsync();

            shop.ShopCategoryLink.Add(link);
            category.ShopCategoryLink.Add(link);
            await AllMarktContextIM.SaveChangesAsync();

            //Act
            var shopList = await _getShopsByCategoryIdQueryHandler.Handle(new GetShopsByCategoryIdQuery { Id = category.Id}, CancellationToken.None);

            //Assert
            shopList.Count().Should().Be(1);
        }
    }
}
