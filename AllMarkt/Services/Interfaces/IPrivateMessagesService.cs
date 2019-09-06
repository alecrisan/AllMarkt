using AllMarkt.Entities;
using AllMarkt.ViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AllMarkt.Services.Interfaces
{
    public interface IPrivateMessagesService
    {
        Task<IEnumerable<PrivateMessageViewModel>> GetAllSentPrivateMessagesByUserAsync(int userId);
        Task<IEnumerable<PrivateMessageViewModel>> GetAllReceivedPrivateMessagesByUserAsync(int userId);
        Task<IEnumerable<PrivateMessageViewModel>> GetAllPrivateMessagesAsync();
        Task<PrivateMessageViewModel> GetPrivateMessageByIdAsync(int id);
        Task SavePrivateMessageAsync(PrivateMessageViewModel viewModel);
        Task DeletePrivateMessageAsync(int id);
        Task UpdateOrDeletePrivateMessage(PrivateMessageViewModel viewModel);
    }
}
