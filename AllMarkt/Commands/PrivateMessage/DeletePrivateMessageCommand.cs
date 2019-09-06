using AllMarkt.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;

namespace AllMarkt.Commands.PrivateMessage
{
    public class DeletePrivateMessageCommand : IRequest
    {
        public int Id { get; set; }
    }

    public class DeletePrivateMessageCommandHandler : AsyncRequestHandler<DeletePrivateMessageCommand>
    {
        private readonly AllMarktContext _allMarktContext;

        public DeletePrivateMessageCommandHandler(AllMarktContext allMarktContext)
        {
            _allMarktContext = allMarktContext;
        }

        protected override async Task Handle(DeletePrivateMessageCommand request, CancellationToken cancellationToken)
        {
            var privateMessage = await _allMarktContext
                .PrivateMessages
                .FirstOrDefaultAsync(x => x.Id == request.Id);

            if (privateMessage != null)
            {
                _allMarktContext.PrivateMessages.Remove(privateMessage);

                await _allMarktContext.SaveChangesAsync(cancellationToken);
            }
        }
    }
}
