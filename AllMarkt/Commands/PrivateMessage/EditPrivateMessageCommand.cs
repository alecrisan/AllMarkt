using AllMarkt.Data;
using AllMarkt.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace AllMarkt.Commands.PrivateMessage
{
    public class EditPrivateMessageCommand : IRequest
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string Text { get; set; }

        public DateTime DateSent { get; set; }

        public DateTime? DateRead { get; set; }

        public int SenderId { get; set; }

        public int ReceiverId { get; set; }

        public DeletedBy? DeletedBy { get; set; }
    }

    public class EditPrivateMessageCommandHandler : AsyncRequestHandler<EditPrivateMessageCommand>
    {
        private readonly AllMarktContext _allMarktContext;

        public EditPrivateMessageCommandHandler(AllMarktContext allMarktContext)
        {
            _allMarktContext = allMarktContext;
        }

        protected override async Task Handle(EditPrivateMessageCommand request, CancellationToken cancellationToken)
        {
            var privateMessage = await _allMarktContext
                .PrivateMessages
                .FirstOrDefaultAsync(PM => PM.Id == request.Id);

            var sender = await _allMarktContext
                .Users
                .FirstOrDefaultAsync(user => user.Id == request.SenderId);

            var receiver = await _allMarktContext
                .Users
                .FirstOrDefaultAsync(user => user.Id == request.ReceiverId);

            if (receiver != null && sender != null)
            {
                privateMessage.Title = request.Title;
                privateMessage.Text = request.Text;
                privateMessage.DateRead = request.DateRead;
                privateMessage.DateSent = request.DateSent;
                privateMessage.Sender = sender;
                privateMessage.Receiver = receiver;
                privateMessage.DeletedBy = request.DeletedBy;

                await _allMarktContext.SaveChangesAsync(cancellationToken);
            }
        }
    }
}
