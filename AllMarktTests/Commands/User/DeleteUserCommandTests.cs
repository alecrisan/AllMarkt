using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AllMarkt.Commands.User;
using AllMarktTests.Queries;
using FluentAssertions;
using MediatR;
using Xunit;

namespace AllMarktTests.Commands.User
{
    public class DeleteUserCommandTests : AllMarktContextTests
    {
        private readonly IRequestHandler<DeleteUserCommand> _deleteUserCommandHandler;

        public DeleteUserCommandTests()
        {
            _deleteUserCommandHandler = new DeleteUserCommandHandler(AllMarktContextIM);
        }

        [Fact]
        public async Task DeleteUserCommandHandler_DeleteExistingUser()
        {
            //Arrange
            var newUser = GetNewUser();
            await AllMarktContextIM.Users.AddAsync(newUser);
            await AllMarktContextIM.SaveChangesAsync(default);

            var existingUser = AllMarktContextIM.Users.First();
            var deleteUserCommand = new DeleteUserCommand { Id = existingUser.Id };

            //Act
            await _deleteUserCommandHandler.Handle(deleteUserCommand, CancellationToken.None);

            //Assert
            AllMarktContextIM.Users
                .Should()
                .NotContain(u => u.Id == deleteUserCommand.Id);
        }

        [Fact]
        public async Task DeleteUserCommandHandler_DeleteExistingUser_And_Asociated_UserRole_AdminObject()
        {
            //Arrange
            var newUser = GetNewUser();       
            await AllMarktContextIM.Users.AddAsync(newUser);
            await AllMarktContextIM.SaveChangesAsync(default);

            var adminUser = new AllMarkt.Entities.Admin { User = newUser };
            await AllMarktContextIM.Admins.AddAsync(adminUser);
            await AllMarktContextIM.SaveChangesAsync(default);

            var existingUser = AllMarktContextIM.Users.First();
            var deleteUserCommand = new DeleteUserCommand { Id = existingUser.Id };

            //Act
            await _deleteUserCommandHandler.Handle(deleteUserCommand, CancellationToken.None);

            //Assert
            AllMarktContextIM.Users
                .Should()
                .NotContain(u => u.Id == deleteUserCommand.Id);

            AllMarktContextIM.Admins
                .Should()
                .BeEmpty();
        }


        private AllMarkt.Entities.User GetNewUser()
        {
            return new AllMarkt.Entities.User
            {
                Email = "email@y.com",
                Password = "123456",
                DisplayName = "name"
            };
        }
    }
}
