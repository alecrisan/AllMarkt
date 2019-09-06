using AllMarkt.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace AllMarkt.Commands.PrivateMessage
{
    public class AddPrivateMessageCommand : IRequest
    {
        public string Title { get; set; }

        public string Text { get; set; }

        public DateTime DateSent { get; set; }

        public DateTime? DateRead { get; set; }

        public int SenderId { get; set; }

        public int ReceiverId { get; set; }
    }

    public class AddPrivateMessageCommandHandler : AsyncRequestHandler<AddPrivateMessageCommand>
    {
        private readonly AllMarktContext _allMarktContext;

        public AddPrivateMessageCommandHandler(AllMarktContext allMarktContext)
        {
            _allMarktContext = allMarktContext;
        }

        protected override async Task Handle(AddPrivateMessageCommand request, CancellationToken cancellationToken)
        {
            Entities.User sender = await _allMarktContext.Users.FirstOrDefaultAsync(user => user.Id == request.SenderId);
            Entities.User receiver = await _allMarktContext.Users.FirstOrDefaultAsync(user => user.Id == request.ReceiverId);

            _allMarktContext.PrivateMessages.Add(
                new Entities.PrivateMessage
                {
                    Title = request.Title,
                    Text = request.Text,
                    DateSent = request.DateSent,
                    DateRead = request.DateRead,
                    Sender = sender,
                    Receiver = receiver
                });

            await _allMarktContext.SaveChangesAsync(cancellationToken);
        }
    }
}
