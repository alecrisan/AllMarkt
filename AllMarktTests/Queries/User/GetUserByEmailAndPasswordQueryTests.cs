using AllMarkt.Entities;
using AllMarkt.Queries.User;
using AllMarkt.ViewModels;
using FluentAssertions;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace AllMarktTests.Queries.User
{
    public class GetUserByEmailAndPasswordQueryTests : AllMarktContextTests
    {
        private readonly IRequestHandler<GetUserByEmailAndPasswordQuery, UserGetViewModel> _getUserByEmailAndPasswordQueryHandler;

        public GetUserByEmailAndPasswordQueryTests()
        {
            _getUserByEmailAndPasswordQueryHandler = new GetUserByEmailAndPasswordQueryHandler(AllMarktQueryContextIM);
        }
        
        [Fact]
        public async Task GetUserByEmailAndPasswordQueryHandler_ReturnsEmpty()
        {
            //Arrange

            //Act
            var result = await _getUserByEmailAndPasswordQueryHandler
                .Handle(new GetUserByEmailAndPasswordQuery { Email = "Email1@yahoo.com", Password = "123456" }, CancellationToken.None);

            //Assert
            result.Should().BeNull();
        }

        [Fact]
        public async Task GetUserByEmailAndPasswordQueryHandler_ReturnsExisting_User()
        {
            //Arrange
            var user = new AllMarkt.Entities.User
            {
                Email = "Email1@yahoo.com",
                Password = AllMarkt.Tools.Hash.ComputeSha256Hash("123456"),
                DisplayName = "name1",
                UserRole = UserRole.Shop
            };

            await AllMarktContextIM.Users.AddAsync(user);
            await AllMarktContextIM.SaveChangesAsync();

            //Act
            var result = await _getUserByEmailAndPasswordQueryHandler
                .Handle(new GetUserByEmailAndPasswordQuery { Email = "Email1@yahoo.com", Password = "123456"}, CancellationToken.None);

            //Assert
            result.Email.Should().Be("Email1@yahoo.com");
            result.DisplayName.Should().Be("name1");
            result.UserRole.Should().Be(nameof(UserRole.Shop));
        }
    }
}
