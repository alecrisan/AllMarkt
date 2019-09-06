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
    public class ShopCategoryController : ControllerBase
    {
        private readonly IShopCategoryService _shopCategoriesService;
        private readonly IClaimsGetter _claimsGetter;
        private readonly IUsersService _usersService;

        public ShopCategoryController(IShopCategoryService shopCategoriesService, IClaimsGetter claimsGetter, IUsersService usersService)
        {
            _shopCategoriesService = shopCategoriesService;
            _claimsGetter = claimsGetter;
            _usersService = usersService;
        }

        [HttpPost("{categoryId}")]
        public async Task<ActionResult> AddAsync(int categoryId)
        {
            var userId = _claimsGetter.UserId(User?.Claims);
            var shop = await _usersService.GetShopByUserIdAsync(userId);
            ShopCategoryViewModel shopCategory = new ShopCategoryViewModel { CategoryId = categoryId, ShopId = shop.Id };

            await _shopCategoriesService.SaveAsync(shopCategory);
            return NoContent();
        }

        [HttpDelete("{categoryId}")]
        public async Task<ActionResult> DeleteAsync(int categoryId)
        {
            var userId = _claimsGetter.UserId(User?.Claims);
            var shop = await _usersService.GetShopByUserIdAsync(userId);
            ShopCategoryViewModel shopCategory = new ShopCategoryViewModel { CategoryId = categoryId, ShopId = shop.Id };
            await _shopCategoriesService.DeleteAsync(shopCategory);
            return NoContent();

        }
    }
}
