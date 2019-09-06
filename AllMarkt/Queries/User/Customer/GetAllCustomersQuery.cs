using AllMarkt.Data;
using AllMarkt.ViewModels;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace AllMarkt.Queries.User.Customer
{
    public class GetAllCustomersQuery: IRequest<IEnumerable<CustomerViewModel>>
    {
    }

    public class GetAllCustomersQueryHandler : IRequestHandler<GetAllCustomersQuery, IEnumerable<CustomerViewModel>>
    {
        private readonly AllMarktQueryContext _allMarktQueryContext;

        public GetAllCustomersQueryHandler(AllMarktQueryContext allMarktQueryContext)
        {
            _allMarktQueryContext = allMarktQueryContext;
        }

        public async Task<IEnumerable<CustomerViewModel>> Handle(GetAllCustomersQuery request, CancellationToken cancellationToken)
        {
            var customers = await _allMarktQueryContext
                .Customers
                .ToListAsync(cancellationToken);

            return from customer in customers
                   select new CustomerViewModel
                   {
                       Id = customer.Id,
                       UserId = customer.UserId,
                       UserDisplayName = customer.User.DisplayName,
                       Address = customer.Address,
                       PhoneNumber = customer.PhoneNumber
                   };
        }
    }
}
