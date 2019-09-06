using AllMarkt.Commands.ShopComments;
using AllMarktTests.Queries;
using FluentAssertions;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace AllMarktTests.Commands.ShopComments
{
    public class AddShopCommentCommandTests : AllMarktContextTests
    {
        private readonly IRequestHandler<AddShopCommentCommand> _addShopCommentCommandHandler;

        public AddShopCommentCommandTests()
        {
            _addShopCommentCommandHandler = new AddShopCommentCommandHandler(AllMarktContextIM);
        }

        [Fact]
        public async Task AddShopCommentCommandHandle_AddsShopComment()
        {
            //Arrange
            var user = new AllMarkt.Entities.User {
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
                SocialCapital = 4
            };

            AllMarktContextIM.Users.Add(user);
            AllMarktContextIM.Shops.Add(shop);
            await AllMarktContextIM.SaveChangesAsync();

            var addShopCommentCommand = new AddShopCommentCommand
            {
                Rating = 5,
                Text = "cel mai bun",
                ShopId = shop.Id,
                AddedByUserId = user.Id
            };

            //Act
            await _addShopCommentCommandHandler.Handle(addShopCommentCommand, CancellationToken.None);

            //Assert
            AllMarktContextIM.ShopComments
                .Should()
                .Contain(shopComment =>
                       shopComment.Rating == addShopCommentCommand.Rating
                       && shopComment.Text == addShopCommentCommand.Text
                       && shopComment.AddedBy.Id == addShopCommentCommand.AddedByUserId
                       && shopComment.Shop.Id == addShopCommentCommand.ShopId);
        }
    }
}
