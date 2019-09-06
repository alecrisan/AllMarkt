using AllMarkt.Commands.ProductComments;
using AllMarktTests.Queries;
using FluentAssertions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace AllMarktTests.Commands.ProductComments
{
    public class DeleteProductCommentsCommandTests : AllMarktContextTests
    {
        private readonly IRequestHandler<DeleteProductCommentCommand> _deleteProductCommentCommandHandler;


        public DeleteProductCommentsCommandTests()
        {
            _deleteProductCommentCommandHandler = new DeleteProductCommentCommandHandler(AllMarktContextIM);
        }

        [Fact]
        public async Task DeleteProductCommentCommandHandle_DeletesExistingProductComment()
        {
            //Arrange
            AllMarktContextIM.ProductComments.Add(new AllMarkt.Entities.ProductComment
            {
                Rating = 5,
                Text = "test"
            });

            await AllMarktContextIM.SaveChangesAsync();

            var existingProductComment = await AllMarktContextIM.ProductComments.FirstAsync();

            var deleteProductCommentCommand = new DeleteProductCommentCommand { Id = existingProductComment.Id };

            //Act
            await _deleteProductCommentCommandHandler.Handle(deleteProductCommentCommand, CancellationToken.None);

            //Assert
            AllMarktContextIM.ProductComments
                .Should()
                .NotContain(ProductComments => ProductComments.Id == deleteProductCommentCommand.Id);
        }
    }
}
