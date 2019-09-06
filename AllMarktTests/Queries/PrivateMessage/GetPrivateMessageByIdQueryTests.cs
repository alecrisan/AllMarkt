using AllMarkt.Entities;
using AllMarkt.Queries.PrivateMessage;
using AllMarkt.ViewModels;
using FluentAssertions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace AllMarktTests.Queries.PrivateMessage
{
    public class GetPrivateMessageByIdQueryTests : AllMarktContextTests
    {
        private readonly IRequestHandler<GetPrivateMessageByIdQuery, PrivateMessageViewModel> _getPrivateMessageByIdQueryHandler;

        public GetPrivateMessageByIdQueryTests()
        {
            _getPrivateMessageByIdQueryHandler = new GetPrivateMessageByIdQueryHandler(AllMarktQueryContextIM);
        }

        [Fact]
        public async Task GetPrivateMessageByIdQueryHandler_ReturnsEmpty()
        {
            //Arrange

            //Act
            var result = await _getPrivateMessageByIdQueryHandler.Handle(new GetPrivateMessageByIdQuery(), CancellationToken.None);

            //Assert
            result.Should().BeNull();
        }

        [Fact]
        public async Task GetPrivateMessageByIdQueryHandler_ReturnsExisting_PrivateMessageById()
        {
            //Arrange
            await Setup_Add2UsersForPrivateMessagesTest();

            AllMarkt.Entities.User user1 = await AllMarktContextIM.Users.FirstOrDefaultAsync(x => x.Email == "mkI@email.com");
            AllMarkt.Entities.User user2 = await AllMarktContextIM.Users.FirstOrDefaultAsync(x => x.Email == "mkII@email.com");

            var message = new AllMarkt.Entities.PrivateMessage
            {
                Title = "Title 1",
                Text = "Text 1",
                DateSent = DateTime.Now,
                DateRead = null,
                Sender = user1,
                Receiver = user2,
                DeletedBy = DeletedBy.Receiver
            };

            await AllMarktContextIM.PrivateMessages.AddAsync(message);
            await AllMarktContextIM.SaveChangesAsync();

            var idAndDisplayNameUser1 = new IdAndDisplayNameUserViewModel(user1);
            var idAndDisplayNameUser2 = new IdAndDisplayNameUserViewModel(user2);

            //Act
            var result = await _getPrivateMessageByIdQueryHandler.Handle(new GetPrivateMessageByIdQuery { Id = message.Id }, CancellationToken.None);

            //Assert
            result.Title.Should().Be("Title 1");
            result.Text.Should().Be("Text 1");
            result.DateRead.Should().Be(null);
            result.Sender.Should().BeEquivalentTo(idAndDisplayNameUser1);
            result.Receiver.Should().BeEquivalentTo(idAndDisplayNameUser2);
            result.DeletedBy.Should().Be(DeletedBy.Receiver);
        }
    }
}
