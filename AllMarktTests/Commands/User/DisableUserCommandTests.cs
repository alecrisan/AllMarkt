using AllMarkt.Commands.User;
using AllMarktTests.Queries;
using FluentAssertions;
using MediatR;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace AllMarktTests.Commands.User
{
    public class DisableUserCommandTests : AllMarktContextTests
    {
        private readonly IRequestHandler<DisableUserCommand> _disableUserCommandHandler;

        public DisableUserCommandTests()
        {
            _disableUserCommandHandler = new DisableUserCommandHandler(AllMarktContextIM);
        }

        [Fact]
        public async Task DisableUserCommandHandler_DisableStatusExistingUser()
        {
            //Arrange
            var newUser = GetNewUser();
            await AllMarktContextIM.Users.AddAsync(newUser);
            await AllMarktContextIM.SaveChangesAsync(default);

            var existingUser = AllMarktContextIM.Users.First();
            var disableUserCommand = new DisableUserCommand { Id = existingUser.Id };

            //Act
            await _disableUserCommandHandler.Handle(disableUserCommand, CancellationToken.None);

            //Assert
            var result = AllMarktContextIM.Users.FirstOrDefault();
            result.IsEnabled.Should().BeFalse();
        }

        private AllMarkt.Entities.User GetNewUser()
        {
            return new AllMarkt.Entities.User
            {
                Email = "email@y.com",
                Password = "123456",
                DisplayName = "name",
                IsEnabled = true
            };
        }
    }
}
