using AllMarkt.Data;
using AllMarkt.ViewModels;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;

namespace AllMarkt.Queries.PrivateMessage
{
    public class GetPrivateMessageByIdQuery : IRequest<PrivateMessageViewModel>
    {
        public int Id { get; set; }
    }

    public class GetPrivateMessageByIdQueryHandler : IRequestHandler<GetPrivateMessageByIdQuery, PrivateMessageViewModel>
    {
        private readonly AllMarktQueryContext _allMarktQueryContext;

        public GetPrivateMessageByIdQueryHandler(AllMarktQueryContext allMarktQueryContext)
        {
            _allMarktQueryContext = allMarktQueryContext;
        }

        public async Task<PrivateMessageViewModel> Handle(GetPrivateMessageByIdQuery request, CancellationToken cancellationToken)
        {
            var privateMessage = await _allMarktQueryContext
                .PrivateMessages
                .FirstOrDefaultAsync(message => message.Id == request.Id);

            if (privateMessage != null)
            {
                return new PrivateMessageViewModel
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
            return null;
        }
    }
}
