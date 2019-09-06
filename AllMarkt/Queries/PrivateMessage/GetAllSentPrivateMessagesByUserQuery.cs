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
    public class GetAllSentPrivateMessagesByUserQuery : IRequest<IEnumerable<PrivateMessageViewModel>>
    {
        public int SenderId { get; set; }

        public GetAllSentPrivateMessagesByUserQuery(int senderId)
        {
            SenderId = senderId;
        }
    }

    public class GetAllSentPrivateMessagesByUserQueryHandler : IRequestHandler<GetAllSentPrivateMessagesByUserQuery, IEnumerable<PrivateMessageViewModel>>
    {
        private readonly AllMarktQueryContext _allMarktQueryContext;

        public GetAllSentPrivateMessagesByUserQueryHandler(AllMarktQueryContext allMarktQueryContext)
        {
            _allMarktQueryContext = allMarktQueryContext;
        }

        public async Task<IEnumerable<PrivateMessageViewModel>> Handle(GetAllSentPrivateMessagesByUserQuery request, CancellationToken cancellationToken)
        {
            var sentPrivateMessages = await _allMarktQueryContext
                .PrivateMessages
                .Where(privateMessage => privateMessage.Sender.Id == request.SenderId)
                .Where(privateMessage => privateMessage.DeletedBy != Entities.DeletedBy.Sender || privateMessage.DeletedBy == null)
                .ToListAsync(cancellationToken);

            return from privateMessage in sentPrivateMessages
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
