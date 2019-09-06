using AllMarkt.Entities;
using AllMarkt.Queries.User.Customer;
using AllMarkt.ViewModels;
using FluentAssertions;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace AllMarktTests.Queries.User
{
    public class GetShopByIdQueryTests : AllMarktContextTests
    {
        private readonly IRequestHandler<GetShopByIdQuery, ShopViewModel> _getShopByIdQueryHandler;

        public GetShopByIdQueryTests()
        {
            _getShopByIdQueryHandler = new GetShopByIdQueryHandler(AllMarktQueryContextIM);
        }

        [Fact]
        public async Task GetShopByIdQueryHandler_ReturnsEmpty()
        {
            //Arrange

            //Act
            var result = await _getShopByIdQueryHandler.Handle(new GetShopByIdQuery(), CancellationToken.None);

            //Assert
            result.Should().BeNull();
        }

        [Fact]
        public async Task GetShopByIdQueryHandler_ReturnsExistingCUI_ShopById()
        {
            //Arrange
            var user1 = new AllMarkt.Entities.User
            {
                Email = "Email1@yahoo.com",
                Password = "123",
                DisplayName = "name1",
                UserRole = UserRole.Shop
            };
            var shop = new AllMarkt.Entities.Shop
            {
                CUI = "3333",
                Address = "address1",
                PhoneNumber = "0147852369",
                Comments = null,
                IBAN = "SE3550000000054910000003",
                Orders = null,
                ProductCategories = null,
                SocialCapital = 3,
                User = user1,
                UserId = user1.Id
            };

            await AllMarktContextIM.Users.AddAsync(user1);
            await AllMarktContextIM.Shops.AddAsync(shop);
            await AllMarktContextIM.SaveChangesAsync();

            //Act
            var result = await _getShopByIdQueryHandler.Handle(new GetShopByIdQuery { Id = shop.Id }, CancellationToken.None);

            //Assert
            result.CUI.Should().Be(shop.CUI);
        }

        [Fact]
        public async Task GetShopByIdQueryHandler_ReturnsExistingIBAN_ShopById()
        {
            //Arrange
            var user1 = new AllMarkt.Entities.User
            {
                Email = "Email1@yahoo.com",
                Password = "123",
                DisplayName = "name1",
                UserRole = UserRole.Shop
            };
            var shop = new AllMarkt.Entities.Shop
            {
                CUI = "3333",
                Address = "address1",
                PhoneNumber = "0147852369",
                Comments = null,
                IBAN = "SE3550000000054910000003",
                Orders = null,
                ProductCategories = null,
                SocialCapital = 3,
                User = user1,
                UserId = user1.Id
            };

            await AllMarktContextIM.Users.AddAsync(user1);
            await AllMarktContextIM.Shops.AddAsync(shop);
            await AllMarktContextIM.SaveChangesAsync();

            //Act
            var result = await _getShopByIdQueryHandler.Handle(new GetShopByIdQuery { Id = shop.Id }, CancellationToken.None);

            //Assert
            result.IBAN.Should().Be(shop.IBAN);
        }

        [Fact]
        public async Task GetShopByIdQueryHandler_ReturnsExistingPhoneNumber_ShopById()
        {
            //Arrange
            var user1 = new AllMarkt.Entities.User
            {
                Email = "Email1@yahoo.com",
                Password = "123",
                DisplayName = "name1",
                UserRole = UserRole.Shop
            };
            var shop = new AllMarkt.Entities.Shop
            {
                CUI = "3333",
                Address = "address1",
                PhoneNumber = "0147852369",
                Comments = null,
                IBAN = "SE3550000000054910000003",
                Orders = null,
                ProductCategories = null,
                SocialCapital = 3,
                User = user1,
                UserId = user1.Id
            };

            await AllMarktContextIM.Users.AddAsync(user1);
            await AllMarktContextIM.Shops.AddAsync(shop);
            await AllMarktContextIM.SaveChangesAsync();

            //Act
            var result = await _getShopByIdQueryHandler.Handle(new GetShopByIdQuery { Id = shop.Id }, CancellationToken.None);

            //Assert
            result.PhoneNumber.Should().Be(shop.PhoneNumber);
        }
        [Fact]
        public async Task GetShopByIdQueryHandler_ReturnsExistingAddress_ShopById()
        {
            //Arrange
            var user1 = new AllMarkt.Entities.User
            {
                Email = "Email1@yahoo.com",
                Password = "123",
                DisplayName = "name1",
                UserRole = UserRole.Shop
            };
            var shop = new AllMarkt.Entities.Shop
            {
                CUI = "3333",
                Address = "address1",
                PhoneNumber = "0147852369",
                Comments = null,
                IBAN = "SE3550000000054910000003",
                Orders = null,
                ProductCategories = null,
                SocialCapital = 3,
                User = user1,
                UserId = user1.Id
            };

            await AllMarktContextIM.Users.AddAsync(user1);
            await AllMarktContextIM.Shops.AddAsync(shop);
            await AllMarktContextIM.SaveChangesAsync();

            //Act
            var result = await _getShopByIdQueryHandler.Handle(new GetShopByIdQuery { Id = shop.Id }, CancellationToken.None);

            //Assert
            result.Address.Should().Be(shop.Address);
        }
    }
}
