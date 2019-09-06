using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AllMarkt.Data;
using AllMarkt.Entities;
using AllMarkt.ViewModels;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace AllMarkt.Queries.User
{
    public class GetAllUsersQuery : IRequest<IEnumerable<UserGetViewModel>>
    {
    }

    public class GetAllUsersQueryHandler : IRequestHandler<GetAllUsersQuery, IEnumerable<UserGetViewModel>>
    {
        private readonly AllMarktQueryContext _allMarktQueryContext;

        public GetAllUsersQueryHandler(AllMarktQueryContext allMarktQueryContext)
        {
            _allMarktQueryContext = allMarktQueryContext;
        }

        public async Task<IEnumerable<UserGetViewModel>> Handle(GetAllUsersQuery request, CancellationToken cancellationToken)
        {
            var users = await _allMarktQueryContext
                .Users
                .ToListAsync(cancellationToken);

            return from user in users
                   select new UserGetViewModel
                   {
                       Id = user.Id,
                       Email = user.Email,
                       Password = user.Password,
                       DisplayName = user.DisplayName,
                       UserRole = Enum.GetName(typeof(UserRole), user.UserRole)
                   };
        }
    }
}
