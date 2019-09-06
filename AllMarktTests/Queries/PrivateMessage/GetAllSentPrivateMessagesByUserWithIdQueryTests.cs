using AllMarkt.Entities;
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
    public class GetAllSentPrivateMessagesByUserWithIdQueryTests : AllMarktContextTests
    {
        private readonly IRequestHandler<GetAllSentPrivateMessagesByUserQuery, IEnumerable<PrivateMessageViewModel>> _getAllSentPrivateMessagesWithUserIdQueryHandler;

        public GetAllSentPrivateMessagesByUserWithIdQueryTests()
        {
            _getAllSentPrivateMessagesWithUserIdQueryHandler = new GetAllSentPrivateMessagesByUserQueryHandler(AllMarktQueryContextIM);
        }

        [Fact]
        public async Task GetAllSentPrivateMessagesQueryHandler_ReturnsEmpty()
        {
            //Arrange
            await Setup_Add2UsersForPrivateMessagesTest();

            AllMarkt.Entities.User user1 = await AllMarktContextIM.Users.FirstOrDefaultAsync(user => user.Email == "mkI@email.com");

            //Act
            var result = await _getAllSentPrivateMessagesWithUserIdQueryHandler.Handle(new GetAllSentPrivateMessagesByUserQuery(user1.Id), CancellationToken.None);

            //Assert
            result.Count().Should().Be(0);
        }

        [Fact]
        public async Task GetAllSentPrivateMessagesQueryHandler_ReturnsExistingPrivateMessagesForUser()
        {
            //Arrange
            await Setup_Add2UsersForPrivateMessagesTest();

            AllMarkt.Entities.User user1 = await AllMarktContextIM.Users.FirstOrDefaultAsync(user => user.Email == "mkI@email.com");
            AllMarkt.Entities.User user2 = await AllMarktContextIM.Users.FirstOrDefaultAsync(user => user.Email == "mkII@email.com");

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
                Title = "Title 2",
                Text = "Text 2",
                DateSent = DateTime.Now,
                DateRead = DateTime.Now,
                Sender = user2,
                Receiver = user2
            });

            await AllMarktContextIM.SaveChangesAsync();

            //Act
            var result = await _getAllSentPrivateMessagesWithUserIdQueryHandler.Handle(new GetAllSentPrivateMessagesByUserQuery(user1.Id), CancellationToken.None);

            //Assert
            result.Count().Should().Be(1);
        }
    }

}
