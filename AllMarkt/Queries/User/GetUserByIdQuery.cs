using System;
using System.Threading;
using System.Threading.Tasks;
using AllMarkt.Data;
using AllMarkt.Entities;
using AllMarkt.ViewModels;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace AllMarkt.Queries.User
{
    public class GetUserByIdQuery : IRequest<UserGetViewModel>
    {
        public int Id { get; set; }
    }

    public class GetUserByIdQueryHandler : IRequestHandler<GetUserByIdQuery, UserGetViewModel>
    {
        private readonly AllMarktQueryContext _allMarktQueryContext;

        public GetUserByIdQueryHandler(AllMarktQueryContext allMarktQueryContext)
        {
            _allMarktQueryContext = allMarktQueryContext;
        }

        public async Task<UserGetViewModel> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
        {
            var userById = await _allMarktQueryContext
                .Users
                .FirstOrDefaultAsync(u => u.Id == request.Id);

            if (userById != null)
            {
                return new UserGetViewModel
                {
                    Id = userById.Id,
                    Email = userById.Email,
                    Password = userById.Password,
                    DisplayName = userById.DisplayName,
                    UserRole = Enum.GetName(typeof(UserRole), userById.UserRole)
                };
            }
            return null;
        }
    }
}
