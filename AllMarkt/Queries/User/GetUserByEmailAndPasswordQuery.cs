using AllMarkt.Data;
using AllMarkt.ViewModels;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;

namespace AllMarkt.Queries.User
{
    public class GetUserByEmailAndPasswordQuery : IRequest<UserGetViewModel>
    {

        public string Email { get; set; }
        public string Password { get; set; }
    }

    public class GetUserByEmailAndPasswordQueryHandler : IRequestHandler<GetUserByEmailAndPasswordQuery, UserGetViewModel>
    {
        private readonly AllMarktQueryContext _allMarktQueryContext;

        public GetUserByEmailAndPasswordQueryHandler(AllMarktQueryContext allMarktQueryContext)
        {
            _allMarktQueryContext = allMarktQueryContext;
        }

        public async Task<UserGetViewModel> Handle(GetUserByEmailAndPasswordQuery request, CancellationToken cancellationToken)
        {
            var hashedPassword = Tools.Hash.ComputeSha256Hash(request.Password);
            var foundUser = await _allMarktQueryContext.Users.FirstOrDefaultAsync(user => user.Email == request.Email && user.Password == hashedPassword);

            if (foundUser == null)
            {
                return null;
            }

            return new UserGetViewModel
            {
                Id = foundUser.Id,
                Email = foundUser.Email,
                Password = foundUser.Password,
                DisplayName = foundUser.DisplayName,
                UserRole = foundUser.UserRole.ToString(),
                IsEnabled = foundUser.IsEnabled
            };
        }
    }
}
