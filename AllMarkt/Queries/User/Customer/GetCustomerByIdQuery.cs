using AllMarkt.Data;
using AllMarkt.ViewModels;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;

namespace AllMarkt.Queries.User.Customer
{
    public class GetCustomerByIdQuery : IRequest<CustomerViewModel>
    {
        public int Id { get; set; }
    }

    public class GetCustomerByIdQueryHandler : IRequestHandler<GetCustomerByIdQuery, CustomerViewModel>
    {
        private readonly AllMarktQueryContext _allMarktQueryContext;

        public GetCustomerByIdQueryHandler(AllMarktQueryContext allMarktQueryContext)
        {
            _allMarktQueryContext = allMarktQueryContext;
        }

        public async Task<CustomerViewModel> Handle(GetCustomerByIdQuery request, CancellationToken cancellationToken)
        {
            var customerById = await _allMarktQueryContext
                .Customers
                .FirstOrDefaultAsync(u => u.Id == request.Id);

            if (customerById != null)
            {
                return new CustomerViewModel
                {
                    Id = customerById.Id,
                    UserDisplayName = customerById.User.DisplayName,
                    Address = customerById.Address,
                    PhoneNumber = customerById.PhoneNumber,
                    UserId = customerById.UserId
                };
            }
            return null;
        }
    }
}
