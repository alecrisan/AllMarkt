using System;
using System.Threading;
using System.Threading.Tasks;
using AllMarkt.Data;
using AllMarkt.Entities;
using MediatR;

namespace AllMarkt.Commands.User
{
    public class AddUserCommand : IRequest
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public string DisplayName { get; set; }
        public string UserRole { get; set; }
    }

    public class AddUserCommandHandler : AsyncRequestHandler<AddUserCommand>
    {
        private readonly AllMarktContext _allMarktContext;

        public AddUserCommandHandler(AllMarktContext allMarktContext)
        {
            _allMarktContext = allMarktContext;
        }

        protected override async Task Handle(AddUserCommand request, CancellationToken cancellationToken)
        {
            var role = char.ToUpper(request.UserRole[0]) + request.UserRole.Substring(1).ToLower();

            var user = new Entities.User
            {
                Email = request.Email,
                Password = request.Password,
                DisplayName = request.DisplayName,
                UserRole = (UserRole)Enum.Parse(typeof(UserRole), role)
            };

            _allMarktContext.Users.Add(user);
            await _allMarktContext.SaveChangesAsync(cancellationToken);

            switch (user.UserRole)
            {
                case UserRole.Admin:
                    {
                        var admin = new Admin { User = user };
                        _allMarktContext.Admins.Add(admin);
                        break;
                    }
                case UserRole.Moderator:
                    {
                        var moderator = new Moderator { User = user };
                        _allMarktContext.Moderators.Add(moderator);
                        break;
                    }
                case UserRole.Shop:
                    {
                        var shop = new Shop { User = user };
                        _allMarktContext.Shops.Add(shop);
                        break;
                    }
                case UserRole.Customer:
                    {
                        var customer = new Customer { User = user };
                        _allMarktContext.Customers.Add(customer);
                        break;
                    }
                default:
                    {
                        break;
                    }
            }
            await _allMarktContext.SaveChangesAsync(cancellationToken);
        }
    }
}
