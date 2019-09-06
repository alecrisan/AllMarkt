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
    public class GetAllPrivateMessagesQuery : IRequest<IEnumerable<PrivateMessageViewModel>>
    {
    }

    public class GetAllPrivateMessagesQueryHandler : IRequestHandler<GetAllPrivateMessagesQuery, IEnumerable<PrivateMessageViewModel>>
    {
        private readonly AllMarktQueryContext _allMarktQueryContext;

        public GetAllPrivateMessagesQueryHandler(AllMarktQueryContext allMarktQueryContext)
        {
            _allMarktQueryContext = allMarktQueryContext;
        }

        public async Task<IEnumerable<PrivateMessageViewModel>> Handle(GetAllPrivateMessagesQuery request, CancellationToken cancellationToken)
        {
            var privateMessages = await _allMarktQueryContext
                .PrivateMessages
                .ToListAsync(cancellationToken);

            return from privateMessage in privateMessages
                   select new PrivateMessageViewModel
                   {
                       Id = privateMessage.Id,
                       Title = privateMessage.Title,
                       Text = privateMessage.Text,
                       DateSent = privateMessage.DateSent,
                       DateRead = privateMessage.DateRead,
                       Sender = new IdAndDisplayNameUserViewModel(privateMessage.Sender),
                       Receiver = new IdAndDisplayNameUserViewModel(privateMessage.Receiver)
                   };
        }
    }
}
