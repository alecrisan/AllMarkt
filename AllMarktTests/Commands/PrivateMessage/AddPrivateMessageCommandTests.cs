using AllMarkt.Commands.PrivateMessage;
using AllMarktTests.Queries;
using FluentAssertions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace AllMarktTests.Commands.PrivateMessage
{
    public class AddPrivateMessageCommandTests : AllMarktContextTests
    {
        private readonly IRequestHandler<AddPrivateMessageCommand> _addPrivateMessageCommandHandler;

        public AddPrivateMessageCommandTests()
        {
            _addPrivateMessageCommandHandler = new AddPrivateMessageCommandHandler(AllMarktContextIM);
        }

        [Fact]
        public async Task AddPrivateMessageCommandHandler_AddsMessage()
        {
            //Arrange
            await Setup_Add2UsersForPrivateMessagesTest();

            AllMarkt.Entities.User user1 = await AllMarktContextIM.Users.FirstOrDefaultAsync(user => user.Email == "mkI@email.com");
            AllMarkt.Entities.User user2 = await AllMarktContextIM.Users.FirstOrDefaultAsync(user => user.Email == "mkII@email.com");

            int before = AllMarktContextIM.PrivateMessages.Count();
            var addPrivateMessageCommand = new AddPrivateMessageCommand
            {
                Title = "1 to 2",
                Text = "Hello",
                DateSent = DateTime.Now,
                DateRead = null,
                SenderId = user1.Id,
                ReceiverId = user2.Id
            };

            //Act
            await _addPrivateMessageCommandHandler.Handle(addPrivateMessageCommand, CancellationToken.None);

            //Assert
            (AllMarktContextIM.PrivateMessages.Count() - before).Should().Be(1);
        }
    }
}
