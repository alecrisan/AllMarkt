using System;
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
    public class GetShopCommentsQueryTests : AllMarktContextTests
    {
        private readonly IRequestHandler<GetShopCommentsByShopQuery, IEnumerable<AllMarkt.ViewModels.ShopCommentViewModel>> _getShopCommentsQueryHandler;

        public GetShopCommentsQueryTests()
        {
            _getShopCommentsQueryHandler = new GetShopCommentsByShopQueryHandler(AllMarktQueryContextIM);
        }

        [Fact]
        public async Task GetShopCommentsQueryHandler_ReturnsEmpty()
        {
            //Arrange

            //Act
            var result = await _getShopCommentsQueryHandler.Handle(new GetShopCommentsByShopQuery(1), CancellationToken.None);

            //Assert
            result.Should().BeEmpty();
        }


        [Fact]
        public async Task GetShopCommentsQueryHandler_ReturnsExistingComments()
        {
            //Arrange
            var user = new AllMarkt.Entities.User
            {
                Email = "test@test.com",
                Password = "123",
                DisplayName = "UserTest"
            };

            var shop1 = new AllMarkt.Entities.Shop
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
            var shop2 = new AllMarkt.Entities.Shop
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
            AllMarktContextIM.Shops.Add(shop1);
            AllMarktContextIM.Shops.Add(shop2);
            await AllMarktContextIM.SaveChangesAsync();

            var shopComment1 = new AllMarkt.Entities.ShopComment
            {
                Rating = 2,
                Text = "naspa rau",
                AddedBy = user,
                Shop = shop1,
                DateAdded = DateTime.UtcNow
            };

            var shopComment2 = new AllMarkt.Entities.ShopComment
            {
                Rating = 4,
                Text = "super fain",
                AddedBy = user,
                Shop = shop2,
                DateAdded = DateTime.UtcNow
            };

            AllMarktContextIM.ShopComments.Add(shopComment1);
            AllMarktContextIM.ShopComments.Add(shopComment2);

            await AllMarktContextIM.SaveChangesAsync();

            //Act
            var result = await _getShopCommentsQueryHandler.Handle(new GetShopCommentsByShopQuery(shop1.Id), CancellationToken.None);

            //Assert
            result.Count().Should().Be(1);
        }
    }
}
