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
    public class GetAllPrivateMessagesQueryTests : AllMarktContextTests
    {
        private readonly IRequestHandler<GetAllPrivateMessagesQuery, IEnumerable<PrivateMessageViewModel>> _getAllPrivateMessagesHandler;

        public GetAllPrivateMessagesQueryTests()
        {
            _getAllPrivateMessagesHandler = new GetAllPrivateMessagesQueryHandler(AllMarktQueryContextIM);
        }

        [Fact]
        public async Task GetAllPrivateMessagesQueryHandler_ReturnsEmpty()
        {
            //Arrange

            //Act
            var result = await _getAllPrivateMessagesHandler.Handle(new GetAllPrivateMessagesQuery(), CancellationToken.None);

            //Assert
            result.Count().Should().Be(0);
        }

        [Fact]
        public async Task GetAllPrivateMessagesQueryHandler_ReturnsExactly2Items()
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
            var result = await _getAllPrivateMessagesHandler.Handle(new GetAllPrivateMessagesQuery(), CancellationToken.None);

            //Assert
            result.Count().Should().Be(2);
        }
    }
}
