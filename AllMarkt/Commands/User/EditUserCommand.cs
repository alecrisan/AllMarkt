using AllMarkt.Data;
using AllMarkt.Entities;
using AllMarkt.Tools;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace AllMarkt.Commands.User
{
    public class EditUserCommand : IRequest
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string OldPassword { get; set; }
        public string NewPassword { get; set; }

        public string DisplayName { get; set; }
        public string UserRole { get; set; }
    }

    public class EditUserCommandHandler : AsyncRequestHandler<EditUserCommand>
    {
        private readonly AllMarktContext _allMarktContext;

        public EditUserCommandHandler(AllMarktContext allMarktContext)
        {
            _allMarktContext = allMarktContext;
        }

        protected override async Task Handle(EditUserCommand request, CancellationToken cancellationToken)
        {
            var user = await _allMarktContext
                .Users
                .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

            if (user != null)
            {
                var curentUserRole = user.UserRole;
                var role = char.ToUpper(request.UserRole[0]) + request.UserRole.Substring(1).ToLower();
                var editUserRole = (UserRole)Enum.Parse(typeof(UserRole), role);

                var requestPasswordHash = Hash.ComputeSha256Hash(request.NewPassword);
                var requestOldPassword = Hash.ComputeSha256Hash(request.OldPassword);
               
                if (requestOldPassword == user.Password && requestPasswordHash != user.Password)
                {
                    user.Password = requestPasswordHash;
                }

                user.Email = request.Email;
                user.DisplayName = request.DisplayName;
                user.UserRole = (UserRole)Enum.Parse(typeof(UserRole), request.UserRole);

                await _allMarktContext.SaveChangesAsync(cancellationToken);

                if (curentUserRole != editUserRole)
                {
                    switch (curentUserRole)
                    {
                        case UserRole.Admin:
                            {
                                var admin = await _allMarktContext.Admins.FirstOrDefaultAsync(x => x.UserId == request.Id, cancellationToken);
                                _allMarktContext.Admins.Remove(admin);
                                break;
                            }
                        case UserRole.Moderator:
                            {
                                var moderator = await _allMarktContext.Moderators.FirstOrDefaultAsync(x => x.UserId == request.Id, cancellationToken);
                                _allMarktContext.Moderators.Remove(moderator);
                                break;
                            }
                        case UserRole.Shop:
                            {
                                var shop = await _allMarktContext.Shops.FirstOrDefaultAsync(x => x.UserId == request.Id, cancellationToken);
                                _allMarktContext.Shops.Remove(shop);
                                break;
                            }
                        case UserRole.Customer:
                            {
                                var customer = await _allMarktContext.Customers.FirstOrDefaultAsync(x => x.UserId == request.Id, cancellationToken);
                                _allMarktContext.Customers.Remove(customer);
                                break;
                            }
                        default:
                            {
                                break;
                            }
                    }
                    await _allMarktContext.SaveChangesAsync(cancellationToken);

                    switch (editUserRole)
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
    }
}
