using AllMarkt.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;

namespace AllMarkt.Commands.User
{
    public class EditShopCommand : IRequest
    {
        public int Id { get; set; }
        public string IBAN { get; set; }
        public string PhoneNumber { get; set; }
        public string Address { get; set; }
    }

    public class EditShopCommandHandler : AsyncRequestHandler<EditShopCommand>
    {
        private readonly AllMarktContext _allMarktContext;

        public EditShopCommandHandler(AllMarktContext allMarktContext)
        {
            _allMarktContext = allMarktContext;
        }

        protected override async Task Handle(EditShopCommand request, CancellationToken cancellationToken)
        {
            var shop = await _allMarktContext
                .Shops
                .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

            if (shop != null)
            {
                shop.Address = request.Address;
                shop.IBAN = request.IBAN;
                shop.PhoneNumber = request.PhoneNumber;

                await _allMarktContext.SaveChangesAsync();
            }
        }
    }
}
