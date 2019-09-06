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
    public class EditShopCommandTests : AllMarktContextTests
    {
        private readonly IRequestHandler<EditShopCommand> _editShopCommandHandler;

        public EditShopCommandTests()
        {
            _editShopCommandHandler = new EditShopCommandHandler(AllMarktContextIM);
        }

        [Fact]
        public async Task EditShop_CommandHandle_UpdatesExistingShop()
        {
            //Arrange
            var shop = new AllMarkt.Entities.Shop
            {
                Address = "address",
                PhoneNumber = "0123654789",
                IBAN = "SE3550000000054910000003"
            };
            AllMarktContextIM.Shops.Add(shop);
            await AllMarktContextIM.SaveChangesAsync();

            var existingShop = AllMarktContextIM.Shops.First();

            var editShopCommand = new EditShopCommand
            {
                Id = existingShop.Id,
                Address = "editedAddress",
                IBAN = "RO3550000000054910000003",
                PhoneNumber = "0147852369"
            };

            //Act
            await _editShopCommandHandler.Handle(editShopCommand, CancellationToken.None);

            //Assert
            AllMarktContextIM.Shops.Should().Contain(x => x.Id == editShopCommand.Id);

            shop.Address.Should().Be(editShopCommand.Address);
            shop.PhoneNumber.Should().Be(editShopCommand.PhoneNumber);
            shop.IBAN.Should().Be(editShopCommand.IBAN);
        }
    }
}