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
    public class EditCustomerCommandTests :  AllMarktContextTests
    {
        private readonly IRequestHandler<EditCustomerCommand> _editCustomerCommandHandler;

        public EditCustomerCommandTests()
        {
            _editCustomerCommandHandler = new EditCustomerCommandHandler(AllMarktContextIM);
        }

        [Fact]
        public async Task EditCustomer_CommandHandle_UpdatesExistingCustomer()
        {
            //Arrange
            var customer = new AllMarkt.Entities.Customer
            {
                Address = "address",
                PhoneNumber = "0123654789"
            };
            AllMarktContextIM.Customers.Add(customer);
            await AllMarktContextIM.SaveChangesAsync();

            var existingCustomer = AllMarktContextIM.Customers.First();

            var editCustomerCommand = new EditCustomerCommand
            {
                Id = existingCustomer.Id,
                Address = "editedAddress",
                PhoneNumber = "0147852369"
            };

            //Act
            await _editCustomerCommandHandler.Handle(editCustomerCommand, CancellationToken.None);

            //Assert
            AllMarktContextIM.Customers.Should().Contain(x => x.Id == editCustomerCommand.Id);

            customer.Address.Should().Be(editCustomerCommand.Address);
            customer.PhoneNumber.Should().Be(editCustomerCommand.PhoneNumber);
        }
    }
}
