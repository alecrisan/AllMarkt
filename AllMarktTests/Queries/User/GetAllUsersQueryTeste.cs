using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AllMarkt.Queries.User;
using AllMarkt.ViewModels;
using FluentAssertions;
using MediatR;
using Xunit;

namespace AllMarktTests.Queries.User
{
    public class GetAllUsersQueryTeste : AllMarktContextTests
    {
        private readonly IRequestHandler<GetAllUsersQuery, IEnumerable<UserGetViewModel>> _getAllUsersQueryHandler;

        public GetAllUsersQueryTeste()
        {
            _getAllUsersQueryHandler = new GetAllUsersQueryHandler(AllMarktQueryContextIM);
        }

        [Fact]
        public async Task GetAllUsersQueryHandler_ReturnsEmpty()
        {
            //Arrange

            //Act
            var result = await _getAllUsersQueryHandler.Handle(new GetAllUsersQuery(), CancellationToken.None);

            //Assert
            result.Should().BeEmpty();
        }

        [Fact]
        public async Task GetAllUsersQueryHandler_ReturnsExistingUsers()
        {
            //Arrange
            var user1 = new AllMarkt.Entities.User
            {
                Email = "Email1@yahoo.com",
                Password = "123",
                DisplayName = "name1"
            };
            var user2 = new AllMarkt.Entities.User
            {
                Email = "Email2@yahoo.com",
                Password = "123",
                DisplayName = "name2"
            };
            await AllMarktContextIM.Users.AddAsync(user1);
            await AllMarktContextIM.Users.AddAsync(user2);

            await AllMarktContextIM.SaveChangesAsync();

            //Act
            var result = await _getAllUsersQueryHandler.Handle(new GetAllUsersQuery(), CancellationToken.None);

            //Assert
            result.Count().Should().Be(2);
        }
    }
}
