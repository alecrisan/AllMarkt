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
    public class ProductCommentsController : ControllerBase
    {
        private readonly IProductCommentsService _productCommentsService;
        private readonly IClaimsGetter _claimsGetter;

        public ProductCommentsController(IProductCommentsService productCommentsService, IClaimsGetter claimsGetter)
        {
            _productCommentsService = productCommentsService;
            _claimsGetter = claimsGetter;
        }

        [AllowAnonymous]
        [HttpGet]
        [Route("{id}")]
        public async Task<ActionResult> GetProductCommentsByProductIdAsync(int id)
        {
            var productComments = await _productCommentsService.GetProductCommentsByProductIdAsync(id);
            return Ok(productComments);
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<ActionResult> GetAllAsync()
        {
            var shopComments = await _productCommentsService.GetAllAsync();
            return Ok(shopComments);
        }

        [HttpPost]
        public async Task<ActionResult> AddAsync(ProductCommentViewModel productComment)
        {
            productComment.AddedById = _claimsGetter.UserId(User?.Claims);
            await _productCommentsService.SaveAsync(productComment);
            return NoContent();
        }

        [Authorize(Roles = "Admin,Moderator")]
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteAsync(int id)
        {
            await _productCommentsService.DeleteAsync(id);
            return NoContent();
        }

        [Authorize(Roles = "Admin,Moderator")]
        [HttpPut]
        public async Task<ActionResult> EditAsync(ProductCommentViewModel productComment)
        {
            await _productCommentsService.SaveAsync(productComment);
            return NoContent();
        }
    }
}
