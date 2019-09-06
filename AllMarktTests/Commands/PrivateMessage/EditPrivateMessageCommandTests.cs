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
    public class EditPrivateMessageCommandTests : AllMarktContextTests
    {
        private readonly IRequestHandler<EditPrivateMessageCommand> _editPrivateMessageCommandHandler;

        public EditPrivateMessageCommandTests()
        {
            _editPrivateMessageCommandHandler = new EditPrivateMessageCommandHandler(AllMarktContextIM);
        }

        [Fact]
        public async Task EditPrivateMessageCommandHandler_UpdatesExistingPrivateMessage()
        {
            //arrange
            await Setup_Add2UsersForPrivateMessagesTest();

            AllMarkt.Entities.User user1 = await AllMarktContextIM.Users.FirstOrDefaultAsync(user => user.Email == "mkI@email.com");
            AllMarkt.Entities.User user2 = await AllMarktContextIM.Users.FirstOrDefaultAsync(user => user.Email == "mkII@email.com");

            var privateMessage = new AllMarkt.Entities.PrivateMessage
            {
                Title = "stock is best medi gun",
                Text = "text",
                DateSent = DateTime.Now,
                DateRead = null,
                Sender = user1,
                Receiver = user2,
                DeletedBy = null
            };

            await AllMarktContextIM.PrivateMessages.AddAsync(privateMessage);
            await AllMarktContextIM.SaveChangesAsync();

            var existingPrivateMessage = await AllMarktContextIM.PrivateMessages.FirstOrDefaultAsync(PM => PM.Title == "stock is best medi gun");

            var editPrivateMessageCommand = new EditPrivateMessageCommand
            {
                Id = existingPrivateMessage.Id,
                Title = "kritzkrieg is best medi gun",
                Text = existingPrivateMessage.Text,
                DateSent = existingPrivateMessage.DateSent,
                DateRead = existingPrivateMessage.DateRead,
                SenderId = existingPrivateMessage.Sender.Id,
                ReceiverId = existingPrivateMessage.Receiver.Id,
                DeletedBy = AllMarkt.Entities.DeletedBy.Receiver
            };

            //Act
            await _editPrivateMessageCommandHandler.Handle(editPrivateMessageCommand, CancellationToken.None);

            //Assert
            AllMarktContextIM.PrivateMessages.Should().Contain(PM => PM.Id == editPrivateMessageCommand.Id);

            privateMessage.Title.Should().Be(editPrivateMessageCommand.Title);
            privateMessage.DeletedBy.Should().Be(editPrivateMessageCommand.DeletedBy);

        }
    }
}
