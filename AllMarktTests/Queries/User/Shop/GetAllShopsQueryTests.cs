using AllMarkt.Queries.Shop;
using AllMarkt.ViewModels;
using FluentAssertions;
using MediatR;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace AllMarktTests.Queries.Shop
{
    public class GetAllShopsQueryTests : AllMarktContextTests
    {
        private readonly IRequestHandler<GetAllShopsQuery, IEnumerable<ShopViewModel>> _getAllShopsQueryHandler;

        public GetAllShopsQueryTests()
        {
            _getAllShopsQueryHandler = new GetAllShopsQueryHandler(AllMarktQueryContextIM);
        }

        [Fact]
        public async Task GetAllShopsQueryHandler_isEmptyAsync()
        {
            //Arrange

            //Act
            var shops = await _getAllShopsQueryHandler.Handle(new GetAllShopsQuery(), CancellationToken.None);

            //Assert
            shops.Should().BeEmpty();
        }

        [Fact]
        public async Task GetAllShopsQueryHandler_IsNotEmpty()
        {
            //Arrange
            var user = new AllMarkt.Entities.User
            {
                Email = "email@asd.com",
                DisplayName = "tets"
            };
            AllMarktContextIM.Users.Add(user);
            await AllMarktContextIM.SaveChangesAsync();

            AllMarktContextIM.Shops.Add(new AllMarkt.Entities.Shop
            {
                ProductCategories = null,
                User = user,  
                Address = "address",
                Comments = null,
                CUI = "RO123456789c",
                IBAN = new string('8', 24),
                PhoneNumber = "01234567891",
                SocialCapital = 23
               
            });
            AllMarktContextIM.Shops.Add(new AllMarkt.Entities.Shop
            {
                ProductCategories = null,
                User = user,
                Address = "address 2",
                Comments = null,
                CUI = "RO12121212c",
                IBAN = new string('6', 24),
                PhoneNumber = "01234567891",
                SocialCapital = 5

            });
            await AllMarktContextIM.SaveChangesAsync();

            //Act
            var shops = await _getAllShopsQueryHandler.Handle(new GetAllShopsQuery(), CancellationToken.None);

            //Assert
            shops.Count().Should().Be(2);
        }
    }
}
