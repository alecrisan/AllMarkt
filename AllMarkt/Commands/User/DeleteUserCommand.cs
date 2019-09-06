using System.Threading;
using System.Threading.Tasks;
using AllMarkt.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace AllMarkt.Commands.User
{
    public class DeleteUserCommand : IRequest
    {
        public int Id { get; set; }
    }

    public class DeleteUserCommandHandler : AsyncRequestHandler<DeleteUserCommand>
    {
        private readonly AllMarktContext _allMarktContext;

        public DeleteUserCommandHandler(AllMarktContext allMarktContext)
        {
            _allMarktContext = allMarktContext;
        }

        protected override async Task Handle(DeleteUserCommand request, CancellationToken cancellationToken)
        {
            var user = await _allMarktContext
                .Users
                .Include(u=>u.ReceivedMessages)
                .Include(u=>u.SentMessages)
                .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

            foreach(var message in user.ReceivedMessages)
            {
                message.Receiver = null;
            }

            foreach(var message in user.SentMessages)
            {
                message.Sender = null;
            }
            await _allMarktContext.SaveChangesAsync(cancellationToken);

            if (user != null)
            {
                _allMarktContext.Remove(user);

                await _allMarktContext.SaveChangesAsync(cancellationToken);
            }
        }
    }
}
