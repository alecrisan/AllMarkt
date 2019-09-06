using AllMarkt.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;

namespace AllMarkt.Commands.User
{
    public class DisableUserCommand : IRequest
    {
        public int Id { get; set; }
    }

    public class DisableUserCommandHandler : AsyncRequestHandler<DisableUserCommand>
    {
        private readonly AllMarktContext _allMarktContext;

        public DisableUserCommandHandler(AllMarktContext allMarktContext)
        {
            _allMarktContext = allMarktContext;
        }

        protected override async Task Handle(DisableUserCommand request, CancellationToken cancellationToken)
        {
            var user = await _allMarktContext
                .Users
                .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

            if (user != null)
            {
                user.IsEnabled = false;

                await _allMarktContext.SaveChangesAsync();
            }
        }
    }
}
