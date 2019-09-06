using AllMarkt.Data;
using AllMarkt.ViewModels;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;

namespace AllMarkt.Queries.User.Customer
{
    public class GetCustomerByUserIdQuery : IRequest<CustomerViewModel>
    {
        public int Id { get; set; }
    }

    public class GetCustomerByUserIdQueryHandler : IRequestHandler<GetCustomerByUserIdQuery, CustomerViewModel>
    {
        private readonly AllMarktQueryContext _allMarktQueryContext;

        public GetCustomerByUserIdQueryHandler(AllMarktQueryContext allMarktQueryContext)
        {
            _allMarktQueryContext = allMarktQueryContext;
        }

        public async Task<CustomerViewModel> Handle(GetCustomerByUserIdQuery request, CancellationToken cancellationToken)
        {
            var customerById = await _allMarktQueryContext
                .Customers
                .FirstOrDefaultAsync(u => u.UserId == request.Id);

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
