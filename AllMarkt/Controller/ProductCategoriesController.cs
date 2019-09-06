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
    public class ProductCategoriesController : ControllerBase
    {
        private readonly IProductCategoriesService _productCategoriesService;
        private readonly IClaimsGetter _claimsGetter;
        private readonly IUsersService _usersService;

        public ProductCategoriesController(IProductCategoriesService productCategoriesService, IClaimsGetter claimsGetter, IUsersService usersService)
        {
            _productCategoriesService = productCategoriesService;
            _claimsGetter = claimsGetter;
            _usersService = usersService;
        }

        [HttpGet]
        public async Task<ActionResult> GetAllAsync()
        {
            var productCategories = await _productCategoriesService.GetAllAsync();
            return Ok(productCategories);
        }

        [HttpGet("shop")]
        public async Task<ActionResult> GetAllByShopIdAsync()
        {
            var userId = _claimsGetter.UserId(User?.Claims);
            var shop = await _usersService.GetShopByUserIdAsync(userId);
            var productCategoriesForShop = await _productCategoriesService.GetAllByShopIdAsync(shop.Id);
            return Ok(productCategoriesForShop);
        }

        [HttpGet("shop/{id}")]
        public async Task<ActionResult> GetAllBySelectedShopAsync(int id)
        {
            var productCategoriesForShop = await _productCategoriesService.GetAllByShopIdAsync(id);
            return Ok(productCategoriesForShop);
        }

        [HttpPost]
        public async Task<ActionResult> AddAsync(ProductCategoryViewModel productCategoryViewModel)
        {
            if(productCategoryViewModel.ShopId == 0)
            {
                var userId = _claimsGetter.UserId(User?.Claims);
                var shop = await _usersService.GetShopByUserIdAsync(userId);
                productCategoryViewModel.ShopId = shop.Id;
            }

            await _productCategoriesService.SaveAsync(productCategoryViewModel);
            return NoContent();
        }

        [HttpPut]
        public async Task<ActionResult> EditAsync(ProductCategoryViewModel productCategoryViewModel)
        {
            await _productCategoriesService.SaveAsync(productCategoryViewModel);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteAsync(int id)
        {
            await _productCategoriesService.DeleteAsync(id);
            return NoContent();
        }
    }
}
