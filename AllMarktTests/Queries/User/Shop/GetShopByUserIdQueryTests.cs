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
    public class GetShopByUserIdQueryTests : AllMarktContextTests
    {
        private readonly IRequestHandler<GetShopByUserIdQuery, ShopViewModel> _getShopByUserIdQueryHandler;

        public GetShopByUserIdQueryTests()
        {
            _getShopByUserIdQueryHandler = new GetShopByUserIdQueryHandler(AllMarktQueryContextIM);
        }

        [Fact]
        public async Task GetUserByIdQueryHandler_ReturnsEmpty()
        {
            //Arrange

            //Act
            var result = await _getShopByUserIdQueryHandler.Handle(new GetShopByUserIdQuery(), CancellationToken.None);

            //Assert
            result.Should().BeNull();
        }

        [Fact]
        public async Task GetUserByIdQueryHandler_ReturnsExisting_UserById()
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
            var result = await _getShopByUserIdQueryHandler.Handle(new GetShopByUserIdQuery { UserId = user1.Id }, CancellationToken.None);

            //Assert
            result.UserDisplayName.Should().Be("name1");
            result.Address.Should().Be("address1");
            result.PhoneNumber.Should().Be("0147852369");
            result.IBAN.Should().Be("SE3550000000054910000003");
            result.SocialCapital.Equals(3);
        }
    }
}
