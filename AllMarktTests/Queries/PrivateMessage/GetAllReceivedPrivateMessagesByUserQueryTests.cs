using AllMarkt.Queries.PrivateMessage;
using AllMarkt.ViewModels;
using FluentAssertions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace AllMarktTests.Queries.PrivateMessage
{
    public class GetAllReceivedPrivateMessagesByUserQueryTests : AllMarktContextTests
    {
        private readonly IRequestHandler<GetAllReceivedPrivateMessagesByUserQuery, IEnumerable<PrivateMessageViewModel>> _getAllReceivedPrivateMessagesWithUserIdQueryHandler;

        public GetAllReceivedPrivateMessagesByUserQueryTests()
        {
            _getAllReceivedPrivateMessagesWithUserIdQueryHandler = new GetAllReceivedPrivateMessagesByUserQueryHandler(AllMarktQueryContextIM);
        }

        [Fact]
        public async Task GetAllReceivedPrivateMessagesQueryHandler_ReturnsEmpty()
        {
            //Arrange
            await Setup_Add2UsersForPrivateMessagesTest();

            AllMarkt.Entities.User user1 = await AllMarktContextIM.Users.FirstOrDefaultAsync(x => x.Email == "mkI@email.com");

            //Act
            var result = await _getAllReceivedPrivateMessagesWithUserIdQueryHandler.Handle(new GetAllReceivedPrivateMessagesByUserQuery(user1.Id), CancellationToken.None);

            //Assert
            result.Count().Should().Be(0);
        }

        [Fact]
        public async Task GetAllReceivedPrivateMessagesQueryHandler_ReturnsExistingPrivateMessagesForUser()
        {
            //Arrange
            await Setup_Add2UsersForPrivateMessagesTest();

            AllMarkt.Entities.User user1 = await AllMarktContextIM.Users.FirstOrDefaultAsync(x => x.Email == "mkI@email.com");
            AllMarkt.Entities.User user2 = await AllMarktContextIM.Users.FirstOrDefaultAsync(x => x.Email == "mkII@email.com");

            AllMarktContextIM.PrivateMessages.Add(new AllMarkt.Entities.PrivateMessage
            {
                Title = "Title 1",
                Text = "Text 1",
                DateSent = DateTime.Now,
                DateRead = DateTime.Now,
                Sender = user1,
                Receiver = user2
            });

            AllMarktContextIM.PrivateMessages.Add(new AllMarkt.Entities.PrivateMessage
            {
                Title = "Title 1",
                Text = "Text 1",
                DateSent = DateTime.Now,
                DateRead = DateTime.Now,
                Sender = user1,
                Receiver = user1
            });

            await AllMarktContextIM.SaveChangesAsync();

            //Act
            var result = await _getAllReceivedPrivateMessagesWithUserIdQueryHandler.Handle(new GetAllReceivedPrivateMessagesByUserQuery(user2.Id), CancellationToken.None);

            //Assert
            result.Count().Should().Be(1);
        }
    }
}
