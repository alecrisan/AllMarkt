using AllMarkt.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;

namespace AllMarkt.Commands.User
{
    public class EditCustomerCommand : IRequest
    {
        public int Id { get; set; }
        public string PhoneNumber { get; set; }
        public string Address { get; set; }
    }

    public class EditCustomerCommandHandler : AsyncRequestHandler<EditCustomerCommand>
    {
        private readonly AllMarktContext _allMarktContext;

        public EditCustomerCommandHandler(AllMarktContext allMarktContext)
        {
            _allMarktContext = allMarktContext;
        }

        protected override async Task Handle(EditCustomerCommand request, CancellationToken cancellationToken)
        {
            var customer = await _allMarktContext
                .Customers
                .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

            if (customer != null)
            {
                customer.Address = request.Address;
                customer.PhoneNumber = request.PhoneNumber;

                await _allMarktContext.SaveChangesAsync();
            }
        }
    }
}
