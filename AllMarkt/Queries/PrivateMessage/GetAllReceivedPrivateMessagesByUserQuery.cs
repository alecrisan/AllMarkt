using AllMarkt.Data;
using AllMarkt.ViewModels;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace AllMarkt.Queries.PrivateMessage
{
    public class GetAllReceivedPrivateMessagesByUserQuery : IRequest<IEnumerable<PrivateMessageViewModel>>
    {
        public int ReceiverId { get; set; }

        public GetAllReceivedPrivateMessagesByUserQuery(int receiverId)
        {
            ReceiverId = receiverId;
        }
    }

    public class GetAllReceivedPrivateMessagesByUserQueryHandler : IRequestHandler<GetAllReceivedPrivateMessagesByUserQuery, IEnumerable<PrivateMessageViewModel>>
    {
        private readonly AllMarktQueryContext _allMarktQueryContext;

        public GetAllReceivedPrivateMessagesByUserQueryHandler(AllMarktQueryContext allMarktQueryContext)
        {
            _allMarktQueryContext = allMarktQueryContext;
        }

        public async Task<IEnumerable<PrivateMessageViewModel>> Handle(GetAllReceivedPrivateMessagesByUserQuery request, CancellationToken cancellationToken)
        {
            var receivedPrivateMessages = await _allMarktQueryContext
                .PrivateMessages
                .Where(privateMessage => privateMessage.Receiver.Id == request.ReceiverId)
                .Where(privateMessage => privateMessage.DeletedBy != Entities.DeletedBy.Receiver || privateMessage.DeletedBy == null)
                .ToListAsync(cancellationToken);

            return from privateMessage in receivedPrivateMessages
                   select new PrivateMessageViewModel
                   {
                       Id = privateMessage.Id,
                       Title = privateMessage.Title,
                       Text = privateMessage.Text,
                       DateSent = privateMessage.DateSent,
                       DateRead = privateMessage.DateRead,
                       Sender = new IdAndDisplayNameUserViewModel(privateMessage.Sender),
                       Receiver = new IdAndDisplayNameUserViewModel(privateMessage.Receiver),
                       DeletedBy = privateMessage.DeletedBy
                   };
        }
    }
}
