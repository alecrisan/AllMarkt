using System;
using System.Threading;
using System.Threading.Tasks;
using AllMarkt.Data;
using AllMarkt.Entities;
using MediatR;

namespace AllMarkt.Commands.User
{
    public class RegisterUserCommand : IRequest
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public string DisplayName { get; set; }
        public string UserRole { get; set; }
        public string IsEnabled { get; set; }
    }

    public class RegisterUserCommandHandler : AsyncRequestHandler<RegisterUserCommand>
    {
        private readonly AllMarktContext _allMarktContext;

        public RegisterUserCommandHandler(AllMarktContext allMarktContext)
        {
            _allMarktContext = allMarktContext;
        }

        protected override async Task Handle(RegisterUserCommand request, CancellationToken cancellationToken)
        {
            var role = char.ToUpper(request.UserRole[0]) + request.UserRole.Substring(1).ToLower();

            var user = new Entities.User
            {
                Email = request.Email,
                Password = request.Password,
                DisplayName = request.DisplayName,
                UserRole = (UserRole)Enum.Parse(typeof(UserRole), role),
                IsEnabled = true
            };

            switch (user.UserRole)
            {
                case UserRole.Shop:
                    {
                        _allMarktContext.Users.Add(user);
                        await _allMarktContext.SaveChangesAsync(cancellationToken);
                        var shop = new Shop { User = user };
                        _allMarktContext.Shops.Add(shop);
                        break;
                    }
                case UserRole.Customer:
                    {
                        _allMarktContext.Users.Add(user);
                        await _allMarktContext.SaveChangesAsync(cancellationToken);
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
