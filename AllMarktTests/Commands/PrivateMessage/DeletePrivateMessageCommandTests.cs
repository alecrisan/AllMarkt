using AllMarkt.Commands.PrivateMessage;
using AllMarktTests.Queries;
using FluentAssertions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace AllMarktTests.Commands.PrivateMessage
{
    public class DeletePrivateMessageCommandTests : AllMarktContextTests
    {
        private readonly IRequestHandler<DeletePrivateMessageCommand> _deletePrivateMessageCommandHandler;

        public DeletePrivateMessageCommandTests()
        {
            _deletePrivateMessageCommandHandler = new DeletePrivateMessageCommandHandler(AllMarktContextIM);
        }

        [Fact]
        public async Task DeletePrivateMessageCommandHandler_DeletesExistingPrivateMessage()
        {
            //Arrange
            await Setup_Add2UsersForPrivateMessagesTest();

            AllMarkt.Entities.User user1 = await AllMarktContextIM.Users.FirstOrDefaultAsync(user => user.Email == "mkI@email.com");
            AllMarkt.Entities.User user2 = await AllMarktContextIM.Users.FirstOrDefaultAsync(user => user.Email == "mkII@email.com");

            AllMarktContextIM.PrivateMessages.Add(
                new AllMarkt.Entities.PrivateMessage
                {
                    Title = "oddly specific title",
                    Text = "text",
                    DateSent = DateTime.Now,
                    DateRead = null,
                    Sender = user1,
                    Receiver = user2
                });

            await AllMarktContextIM.SaveChangesAsync();

            var existingPrivateMessage = AllMarktContextIM.PrivateMessages.FirstOrDefaultAsync(user => user.Title == "oddly specific title");

            var deletePrivateMessageCommand = new DeletePrivateMessageCommand { Id = existingPrivateMessage.Id };

            //Act
            await _deletePrivateMessageCommandHandler.Handle(deletePrivateMessageCommand, CancellationToken.None);

            //Assert
            AllMarktContextIM.PrivateMessages
                .Should()
                .NotContain(privateMessage => privateMessage.Id == existingPrivateMessage.Id);
        }
    }
}
