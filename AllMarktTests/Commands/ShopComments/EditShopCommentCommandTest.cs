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
    public class EditShopCommentCommandTest : AllMarktContextTests
    {
        private readonly IRequestHandler<EditShopCommentCommand> _editShopCommentCommandHandler;


        public EditShopCommentCommandTest()
        {
            _editShopCommentCommandHandler = new EditShopCommentCommandHandler(AllMarktContextIM);
        }

        [Fact]
        public async Task EditShopCommentCommandHandle_UpdatesExistingShopComment()
        {
            //Arrange
            var shopComment = new AllMarkt.Entities.ShopComment
            {
                Rating = 3,
                Text = "test"
            };

            await AllMarktContextIM.ShopComments.AddAsync(shopComment);
            await AllMarktContextIM.SaveChangesAsync();

            var existingShopComments = await AllMarktContextIM.ShopComments.FirstAsync();

            var editShopCommentsCommand = new EditShopCommentCommand
            {
                Id = existingShopComments.Id,
                Text = "test_edit"
            };

            //Act
            await _editShopCommentCommandHandler.Handle(editShopCommentsCommand, CancellationToken.None);

            //Assert
            AllMarktContextIM.ShopComments
                .Should()
                .Contain(x => x.Id == editShopCommentsCommand.Id && x.Text == editShopCommentsCommand.Text);
        }
    }
}
