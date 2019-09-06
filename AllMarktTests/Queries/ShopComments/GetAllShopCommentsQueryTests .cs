using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AllMarkt.Queries.ShopComments;
using FluentAssertions;
using MediatR;
using Xunit;

namespace AllMarktTests.Queries.ShopComments
{
    public class GetAllShopCommentsQueryTests : AllMarktContextTests
    {
        private readonly IRequestHandler<GetAllShopCommentsQuery, IEnumerable<AllMarkt.ViewModels.ShopCommentViewModel>> _getAllShopCommentsQueryHandler;

        public GetAllShopCommentsQueryTests()
        {
            _getAllShopCommentsQueryHandler = new GetAllShopCommentsQueryHandler(AllMarktQueryContextIM);
        }

        [Fact]
        public async Task GetAllShopCommentsQueryHandler_ReturnsEmpty()
        {
            //Arrange

            //Act
            var result = await _getAllShopCommentsQueryHandler.Handle(new GetAllShopCommentsQuery(), CancellationToken.None);

            //Assert
            result.Should().BeEmpty();
        }

        [Fact]
        public async Task GetAllShopCommentsQueryHandler_ReturnsExistingComments()
        {
            //Arrange
            var user = new AllMarkt.Entities.User
            {
                Email = "test@test.com",
                Password = "123",
                DisplayName = "UserTest"
            };

            var shop = new AllMarkt.Entities.Shop
            {
                Address = "aaaaa",
                Comments = null,
                CUI = "aaaaddd",
                IBAN = "aaaaa",
                Orders = null,
                PhoneNumber = "0123654789",
                ProductCategories = null,
                ShopCategoryLink = null,
                SocialCapital = 4,
                User = user
            };

            AllMarktContextIM.Users.Add(user);
            AllMarktContextIM.Shops.Add(shop);
            await AllMarktContextIM.SaveChangesAsync();

            AllMarktContextIM.ShopComments.Add(new AllMarkt.Entities.ShopComment
            {
                Rating = 2,
                Text = "naspa rau",
                AddedBy = user,
                Shop = shop,
            });

            AllMarktContextIM.ShopComments.Add(new AllMarkt.Entities.ShopComment
            {
                Rating = 5,
                Text = "super fain",
                AddedBy = user,
                Shop = shop
            });

            await AllMarktContextIM.SaveChangesAsync();

            //Act
            var result = await _getAllShopCommentsQueryHandler.Handle(new GetAllShopCommentsQuery(), CancellationToken.None);

            //Assert
            result.Count().Should().Be(2);
        }
    }
}
