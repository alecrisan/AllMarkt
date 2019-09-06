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
    public class ShopCommentsController : ControllerBase
    {
        private readonly IShopCommentsService _shopCommentsService;
        private readonly IClaimsGetter _claimsGetter;

        public ShopCommentsController(IShopCommentsService shopCommentsService, IClaimsGetter claimsGetter)
        {
            _shopCommentsService = shopCommentsService;
            _claimsGetter = claimsGetter;
        }

        [AllowAnonymous]
        [HttpGet]
        [Route("{id}")]
        public async Task<ActionResult> GetShopCommentsByShopIdAsync(int id)
        {
            var shopComments = await _shopCommentsService.GetShopCommentsByShopId(id);
            return Ok(shopComments);
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<ActionResult> GetAllAsync()
        {
            var shopComments = await _shopCommentsService.GetAllAsync();
            return Ok(shopComments);
        }

        [HttpPost]
        public async Task<ActionResult> AddAsync(ShopCommentViewModel shopComment)
        {

            shopComment.AddedById = _claimsGetter.UserId(User?.Claims);
            await _shopCommentsService.SaveAsync(shopComment);
            return NoContent();
        }

        [Authorize(Roles = "Admin,Moderator")]
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteAsync(int id)
        {
            await _shopCommentsService.DeleteAsync(id);
            return NoContent();
        }

        [Authorize(Roles = "Admin,Moderator")]
        [HttpPut]
        public async Task<ActionResult> EditAsync(ShopCommentViewModel shopComment)
        {
            await _shopCommentsService.SaveAsync(shopComment);
            return NoContent();
        }
    }
}
