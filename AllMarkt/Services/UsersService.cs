using AllMarkt.Commands.User;
using AllMarkt.Queries.Shop;
using AllMarkt.Queries.User;
using AllMarkt.Queries.User.Customer;
using AllMarkt.Queries.User.Shop;
using AllMarkt.Services.Interfaces;
using AllMarkt.Tools;
using AllMarkt.ViewModels;
using MediatR;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace AllMarkt.Services
{
    public class UsersService : IUsersService
    {
        private readonly IMediator _mediator;
        private readonly AppSettings _appSettings;
        private readonly ITokenGenerator _tokenGenerator;

        public UsersService(IMediator mediator, IOptions<AppSettings> appSettings, ITokenGenerator tokenGenerator)
        {
            _mediator = mediator;
            _appSettings = appSettings.Value;
            _tokenGenerator = tokenGenerator;
        }

        public Task<IEnumerable<UserGetViewModel>> GetAllAsync()
            => _mediator.Send(new GetAllUsersQuery());


        public Task<UserGetViewModel> GetByIdAsync(int id)
        {
            return _mediator.Send(new GetUserByIdQuery
            {
                Id = id
            });
        }

        public Task<ShopViewModel> GetShopByUserIdAsync(int userId)
        {
            return _mediator.Send(new GetShopByUserIdQuery
            {
                UserId = userId
            });
        }

        public Task<ShopViewModel> GetShopByIdAsync(int id)
        {
            return _mediator.Send(new GetShopByIdQuery
            {
                Id = id
            });
        }

        public Task<CustomerViewModel> GetCustomerByUserIdAsync(int userId)
        {
            return _mediator.Send(new GetCustomerByUserIdQuery
            {
                Id = userId
            });
        }

        public Task<CustomerViewModel> GetCustomerByIdAsync(int id)
        {
            return _mediator.Send(new GetCustomerByIdQuery
            {
                Id = id
            });
        }

        public async Task SaveAsync(UserInputViewModel userPostViewModel)
        {
            await AddUserAsync(userPostViewModel);
        }

        private Task AddUserAsync(UserInputViewModel userViewModel)
        {
            var addUserCommand = new AddUserCommand
            {
                Email = userViewModel.Email,
                Password = Hash.ComputeSha256Hash(userViewModel.Password),
                DisplayName = userViewModel.DisplayName,
                UserRole = userViewModel.UserRole
            };

            return _mediator.Send(addUserCommand, default(CancellationToken));
        }

        public Task EditAsync(UserEditViewModel userViewModel)
        {
            var editUserCommand = new EditUserCommand
            {
                Id = userViewModel.Id,
                Email = userViewModel.Email,
                OldPassword = userViewModel.OldPassword,
                NewPassword = userViewModel.NewPassword,
                DisplayName = userViewModel.DisplayName,
                UserRole = userViewModel.UserRole
            };

            return _mediator.Send(editUserCommand, default(CancellationToken));
        }

        public Task DeleteAsync(int id)
        {
            return _mediator.Send(new DeleteUserCommand
            {
                Id = id
            });
        }

        public Task<IEnumerable<ShopViewModel>> GetAllShopsAsync()
            => _mediator.Send(new GetAllShopsQuery());

        public Task EditShopAsync(ShopViewModel shopViewModel)
        {
            var editShopCommand = new EditShopCommand
            {
                Id = shopViewModel.Id,
               Address = shopViewModel.Address,
               IBAN = shopViewModel.IBAN,
               PhoneNumber = shopViewModel.PhoneNumber
            };

            return _mediator.Send(editShopCommand, default(CancellationToken));
        }

        public Task<IEnumerable<CustomerViewModel>> GetAllCustomersAsync()
            => _mediator.Send(new GetAllCustomersQuery());
            
        public Task<IEnumerable<ShopViewModel>> GetShopsByCategoryIdAsync(int id)
            => _mediator.Send(new GetShopsByCategoryIdQuery { Id=id });

        public async Task<UserLoginResponseViewModel> Authenticate(string email, string password)
        {
            var user = await _mediator.Send(new GetUserByEmailAndPasswordQuery { Email = email, Password = password });

            if (user == null)
                return null;

            if (user.IsEnabled == false)
            {
                return null;
            }

            var userData = new UserTokenDataModel { Id = user.Id, Email = email, DisplayName = user.DisplayName, UserRole = user.UserRole };

            var token = _tokenGenerator.Generate(_appSettings, userData);

            return new UserLoginResponseViewModel
            {
                Token = token,
                Email = user.Email,
                DisplayName = user.DisplayName,
                UserRole = user.UserRole
            };
        }
        
        public Task EditCustomerAsync(CustomerViewModel customerViewModel)
        {
            var editCustomerCommand = new EditCustomerCommand
            {
               Id = customerViewModel.Id,
               PhoneNumber = customerViewModel.PhoneNumber,
               Address = customerViewModel.Address
            };

            return _mediator.Send(editCustomerCommand, default(CancellationToken));
        }

        public Task RegisterAsync(UserInputViewModel userViewModel)
        {
            var registerUserCommand = new RegisterUserCommand
            {
                Email = userViewModel.Email,
                Password = Hash.ComputeSha256Hash(userViewModel.Password),
                DisplayName = userViewModel.DisplayName,
                UserRole = userViewModel.UserRole
            };

            return _mediator.Send(registerUserCommand, default);
        }

        public Task DisableUserByIdAsync(UserEditViewModel userEditViewModel)
        {
            var disableUserCommand = new DisableUserCommand
            {
                Id = userEditViewModel.Id
            };
        return _mediator.Send(disableUserCommand, default(CancellationToken));
        }
    }
}
