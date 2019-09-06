using System;
using System.Threading;
using System.Threading.Tasks;
using AllMarkt.Commands.User;
using AllMarkt.Entities;
using AllMarktTests.Queries;
using FluentAssertions;
using MediatR;
using Xunit;

namespace AllMarktTests.Commands.User
{
    public class EditUserCommandTests : AllMarktContextTests
    {
        private readonly IRequestHandler<EditUserCommand> _editUserCommandHandler;

        public EditUserCommandTests()
        {
            _editUserCommandHandler = new EditUserCommandHandler(AllMarktContextIM);
        }

        [Fact]
        public async Task EditUserCommandHandler_Change_CustomerUser_Into_AdminUser()
        {
            //Arrange
            var user = await CreateUserWithRoleAsync(UserRole.Customer);

            var customer = new Customer
            {
                User = user
            };
            await AllMarktContextIM.Customers.AddAsync(customer);
            await AllMarktContextIM.SaveChangesAsync();

            var editUserCommand = CreateEditUserCommand(user.Id, "Admin");

            //Act
            await _editUserCommandHandler.Handle(editUserCommand, CancellationToken.None);

            //Assert
            AllMarktContextIM.Users.Should()
                .Contain(u =>
                u.Email == editUserCommand.Email &&
                u.DisplayName == editUserCommand.DisplayName &&
                u.UserRole == (UserRole)Enum.Parse(typeof(UserRole), editUserCommand.UserRole)
                );
            var customerFound = await AllMarktContextIM.Customers.FindAsync(user.Id);
            customerFound.Should().BeNull();

            AllMarktContextIM.Admins.Should().Contain(a => a.UserId == user.Id);
        }

        [Fact]
        public async Task EditUserCommandHandler_Change_ModeratorUser_Into_ShopUser()
        {
            //Arrange
            var user = await CreateUserWithRoleAsync(UserRole.Moderator);

            var moderator = new Moderator
            {
                User = user
            };
            await AllMarktContextIM.Moderators.AddAsync(moderator);
            await AllMarktContextIM.SaveChangesAsync();

            var editUserCommand = CreateEditUserCommand(user.Id, "Shop");

            //Act
            await _editUserCommandHandler.Handle(editUserCommand, CancellationToken.None);

            //Assert
            AllMarktContextIM.Users.Should()
                .Contain(u =>
                u.Email == editUserCommand.Email &&
                u.DisplayName == editUserCommand.DisplayName &&
                u.UserRole == (UserRole)Enum.Parse(typeof(UserRole), editUserCommand.UserRole)
                );
            var moderatorFound = await AllMarktContextIM.Moderators.FindAsync(user.Id);
            moderatorFound.Should().BeNull();

            AllMarktContextIM.Shops.Should().Contain(a => a.UserId == user.Id);
        }

        [Fact]
        public async Task EditUserCommandHandler_Change_ShopUser_Into_AdminUser()
        {
            //Arrange
            var user = await CreateUserWithRoleAsync(UserRole.Shop);

            var shop = new Shop
            {
                User = user
            };
            await AllMarktContextIM.Shops.AddAsync(shop);
            await AllMarktContextIM.SaveChangesAsync();

            var editUserCommand = CreateEditUserCommand(user.Id, "Admin");

            //Act
            await _editUserCommandHandler.Handle(editUserCommand, CancellationToken.None);

            //Assert
            AllMarktContextIM.Users.Should()
                .Contain(u =>
                u.Email == editUserCommand.Email &&
                u.DisplayName == editUserCommand.DisplayName &&
                u.UserRole == (UserRole)Enum.Parse(typeof(UserRole), editUserCommand.UserRole)
                );
            var shopFound = await AllMarktContextIM.Shops.FindAsync(user.Id);
            shopFound.Should().BeNull();

            AllMarktContextIM.Admins.Should().Contain(a => a.UserId == user.Id);
        }

        private async Task<AllMarkt.Entities.User> CreateUserWithRoleAsync(UserRole role)
        {
            var user1 = new AllMarkt.Entities.User
            {
                Email = "email@yahoo.com",
                Password = "123",
                DisplayName = "Name",
                UserRole = role
            };
            await AllMarktContextIM.Users.AddAsync(user1);
            await AllMarktContextIM.SaveChangesAsync();
            return user1;
        }

        private EditUserCommand CreateEditUserCommand(int id, string userRole)
        {
            var editUserCommand = new EditUserCommand
            {
                Id = id,
                Email = "schimbat@yahoo.com",
                OldPassword = "123",
                NewPassword = "abc",
                DisplayName = "numeSchimbat",
                UserRole = userRole
            };
            return editUserCommand;
        }
    }
}
