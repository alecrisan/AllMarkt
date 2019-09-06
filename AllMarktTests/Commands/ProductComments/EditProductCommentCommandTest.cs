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
    public class EditProductCommentCommandTest : AllMarktContextTests
    {
        private readonly IRequestHandler<EditProductCommentCommand> _editProductCommentCommandHandler;


        public EditProductCommentCommandTest()
        {
            _editProductCommentCommandHandler = new EditProductCommentCommandHandler(AllMarktContextIM);
        }

        [Fact]
        public async Task EditProductCommentCommandHandle_UpdatesExistingProductComment()
        {
            //Arrange
            var productComment = new AllMarkt.Entities.ProductComment
            {
                Rating = 3,
                Text = "test"
            };

            await AllMarktContextIM.ProductComments.AddAsync(productComment);
            await AllMarktContextIM.SaveChangesAsync();

            var existingProductComments = await AllMarktContextIM.ProductComments.FirstAsync();

            var editProductCommentsCommand = new EditProductCommentCommand
            {
                Id = existingProductComments.Id,
                Text = "test_edit"
            };

            //Act
            await _editProductCommentCommandHandler.Handle(editProductCommentsCommand, CancellationToken.None);

            //Assert
            AllMarktContextIM.ProductComments
                .Should()
                .Contain(x => x.Id == editProductCommentsCommand.Id && x.Text == editProductCommentsCommand.Text);
        }
    }
}
