using AllMarkt.Services.Interfaces;
using AllMarkt.Tools;
using AllMarkt.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace AllMarkt.Controller
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class PrivateMessagesController : ControllerBase
    {
        private readonly IPrivateMessagesService _privateMessagesService;
        private readonly IClaimsGetter _claimsGetter;

        public PrivateMessagesController(IPrivateMessagesService privateMessagesService, IClaimsGetter claimsGetter)
        {
            _privateMessagesService = privateMessagesService;
            _claimsGetter = claimsGetter;
        }

        [HttpGet("asSender")]
        public async Task<ActionResult> GetAllSentPrivateMessagesByUserAsync()
        {
            var userId = _claimsGetter.UserId(User?.Claims);
            var privateMessages = await _privateMessagesService.GetAllSentPrivateMessagesByUserAsync(userId);
            return Ok(privateMessages);
        }

        [HttpGet("asReceiver")]
        public async Task<ActionResult> GetAllReceivedPrivateMessagesByUserAsync()
        {
            var userId = _claimsGetter.UserId(User?.Claims);
            var privateMessages = await _privateMessagesService.GetAllReceivedPrivateMessagesByUserAsync(userId);
            return Ok(privateMessages);
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<ActionResult> GetAllPrivateMessagesAsync()
        {
            var privateMessages = await _privateMessagesService.GetAllPrivateMessagesAsync();
            return Ok(privateMessages);
        }

        [HttpPost]
        public async Task<ActionResult> AddPrivateMessageAsync(PrivateMessageViewModel privateMessage)
        {
            privateMessage.Sender.Id = _claimsGetter.UserId(User?.Claims);
            await _privateMessagesService.SavePrivateMessageAsync(privateMessage);
            return NoContent();
        }
        
        [HttpPut]
        [Route("[action]")]
        public async Task<ActionResult> EditPrivateMessageAsync(PrivateMessageViewModel privateMessage)
        {
            await _privateMessagesService.SavePrivateMessageAsync(privateMessage);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeletePrivateMessageAsync(int id)
        {
            await _privateMessagesService.DeletePrivateMessageAsync(id);
            return NoContent();
        }

        [HttpPut]
        [Route("[action]")]
        public async Task<ActionResult> UpdatePrivateMessageStatusAsync(PrivateMessageViewModel privateMessage)
        {
            await _privateMessagesService.UpdateOrDeletePrivateMessage(privateMessage);
            return NoContent();
        }
    }
}