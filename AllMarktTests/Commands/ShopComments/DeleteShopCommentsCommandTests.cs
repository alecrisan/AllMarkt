using AllMarkt.Commands.ShopComments;
using AllMarktTests.Queries;
using FluentAssertions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace AllMarktTests.Commands.ShopComments
{
    public class DeleteShopCommentsCommandTests : AllMarktContextTests
    {
        private readonly IRequestHandler<DeleteShopCommentCommand> _deleteShopCommentCommandHandler;


        public DeleteShopCommentsCommandTests()
        {
            _deleteShopCommentCommandHandler = new DeleteShopCommentCommandHandler(AllMarktContextIM);
        }

        [Fact]
        public async Task DeleteShopCommentCommandHandle_DeletesExistingShopComment()
        {
            //Arrange
            AllMarktContextIM.ShopComments.Add(new AllMarkt.Entities.ShopComment
            {
                Rating = 5,
                Text = "test"
            });

            await AllMarktContextIM.SaveChangesAsync();

            var existingShopComment = await AllMarktContextIM.ShopComments.FirstAsync();

            var deleteShopCommentCommand = new DeleteShopCommentCommand { Id = existingShopComment.Id };

            //Act
            await _deleteShopCommentCommandHandler.Handle(deleteShopCommentCommand, CancellationToken.None);

            //Assert
            AllMarktContextIM.ShopComments
                .Should()
                .NotContain(ShopComments => ShopComments.Id == deleteShopCommentCommand.Id);
        }
    }
}
