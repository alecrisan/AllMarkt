using AllMarkt.Commands.ProductComments;
using AllMarktTests.Queries;
using FluentAssertions;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace AllMarktTests.Commands.ProductComments
{
    public class AddProductCommentCommandTests : AllMarktContextTests
    {
        private readonly IRequestHandler<AddProductCommentCommand> _addProductCommentCommandHandler;

        public AddProductCommentCommandTests()
        {
            _addProductCommentCommandHandler = new AddProductCommentCommandHandler(AllMarktContextIM);
        }

        [Fact]
        public async Task AddProductCommentCommandHandle_AddsShopComment()
        {
            //Arrange
            var productCategory = new AllMarkt.Entities.ProductCategory { Name = "category", Description = "description" };
            AllMarktContextIM.ProductCategories.Add(productCategory);
            AllMarktContextIM.SaveChanges();

            var user = new AllMarkt.Entities.User {
                Email = "test@test.com",
                Password = "123",
                DisplayName = "UserTest"
            };

            var product = new AllMarkt.Entities.Product
            {
                Name = "testProduct",
                Description = "testDescription",
                Price = 20,
                ImageURI = "",
                State = true,
                ProductCategory = productCategory
            };

            AllMarktContextIM.Users.Add(user);
            AllMarktContextIM.Products.Add(product);
            await AllMarktContextIM.SaveChangesAsync();

            var addProductCommentCommand = new AddProductCommentCommand
            {
                Rating = 5,
                Text = "cel mai bun produs",
                ProductId = product.Id,
                AddedByUserId = user.Id
            };

            //Act
            await _addProductCommentCommandHandler.Handle(addProductCommentCommand, CancellationToken.None);

            //Assert
            AllMarktContextIM.ProductComments
                .Should()
                .Contain(productComment =>
                       productComment.Rating == addProductCommentCommand.Rating
                       && productComment.Text == addProductCommentCommand.Text
                       && productComment.AddedBy.Id == addProductCommentCommand.AddedByUserId
                       && productComment.Product.Id == addProductCommentCommand.ProductId);
        }
    }
}
