using AllMarkt.Commands.PrivateMessage;
using AllMarkt.Queries.PrivateMessage;
using AllMarkt.Services.Interfaces;
using AllMarkt.ViewModels;
using MediatR;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AllMarkt.Services
{
    public class PrivateMessagesService : IPrivateMessagesService
    {
        private readonly IMediator _mediator;

        public PrivateMessagesService(IMediator mediator)
        {
            _mediator = mediator;
        }

        public Task<IEnumerable<PrivateMessageViewModel>> GetAllSentPrivateMessagesByUserAsync(int senderId)
            => _mediator.Send(new GetAllSentPrivateMessagesByUserQuery(senderId));

        public Task<IEnumerable<PrivateMessageViewModel>> GetAllReceivedPrivateMessagesByUserAsync(int senderId)
            => _mediator.Send(new GetAllReceivedPrivateMessagesByUserQuery(senderId));

        public Task<IEnumerable<PrivateMessageViewModel>> GetAllPrivateMessagesAsync()
            => _mediator.Send(new GetAllPrivateMessagesQuery());

        public Task<PrivateMessageViewModel> GetPrivateMessageByIdAsync(int id)
            => _mediator.Send(new GetPrivateMessageByIdQuery
            {
                Id = id
            });

        public Task SavePrivateMessageAsync(PrivateMessageViewModel viewModel)
            => viewModel.Id == 0 ? AddPrivateMessageAsync(viewModel) : EditPrivateMessageAsync(viewModel);

        private Task AddPrivateMessageAsync(PrivateMessageViewModel viewModel)
            => _mediator.Send(new AddPrivateMessageCommand
            {
                Title = viewModel.Title,
                Text = viewModel.Text,
                DateSent = viewModel.DateSent,
                DateRead = viewModel.DateRead,
                SenderId = viewModel.Sender.Id,
                ReceiverId = viewModel.Receiver.Id
            });

        private Task EditPrivateMessageAsync(PrivateMessageViewModel viewModel)
            => _mediator.Send(new EditPrivateMessageCommand
            {
                Id = viewModel.Id,
                Title = viewModel.Title,
                Text = viewModel.Text,
                DateSent = viewModel.DateSent,
                DateRead = viewModel.DateRead,
                SenderId = viewModel.Sender.Id,
                ReceiverId = viewModel.Receiver.Id,
                DeletedBy = viewModel.DeletedBy
            });

        public async Task UpdateOrDeletePrivateMessage(PrivateMessageViewModel viewModel)
        {
            var existingMessage = await GetPrivateMessageByIdAsync(viewModel.Id);

            if (existingMessage.DeletedBy != null && existingMessage.DeletedBy != viewModel.DeletedBy)
                await DeletePrivateMessageAsync(existingMessage.Id);
            else
                await EditPrivateMessageAsync(viewModel);
        }

        public Task DeletePrivateMessageAsync(int id)
            => _mediator.Send(new DeletePrivateMessageCommand
            {
                Id = id
            });
    }
}
