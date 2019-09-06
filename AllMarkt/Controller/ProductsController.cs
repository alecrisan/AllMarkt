using AllMarkt.Services.Interfaces;
using AllMarkt.Tools;
using AllMarkt.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace AllMarkt.Controller
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IProductsService _productsService;
        private readonly IClaimsGetter _claimsGetter;
        private readonly IUsersService _usersService;

        public ProductsController(IProductsService productsService, IClaimsGetter claimsGetter, IUsersService usersService)
        {
            _productsService = productsService;
            _claimsGetter = claimsGetter;
            _usersService = usersService;
        }

        [HttpGet]
        public async Task<ActionResult> GetAllAsync()
        {
            var products = await _productsService.GetAllAsync();
            return Ok(products);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> GetAllByProductCategoryAsync(int id)
        {
            var products = await _productsService.GetAllByProductCategoryAsync(id);
            return Ok(products);
        }
        
        [HttpGet("withRatingId={id}")]
        public async Task<ActionResult> GetProductsWithRatingByCategoryAsync(int id)
        {
            var products = await _productsService.GetProductWithRatingByCategoryAsync(id);
            return Ok(products);
        }

        [HttpGet("shop")]
        public async Task<ActionResult> GetAllProductsByShopAsync()
        {
            var userId = _claimsGetter.UserId(User?.Claims);
            var shop =  await _usersService.GetShopByUserIdAsync(userId);
            var products = await _productsService.GetAllProductsByShopAsync(shop.Id);
            return Ok(products);
        }

        [HttpGet("selectedShop={id}")]
        public async Task<ActionResult> GetAllProductsBySelectedShopAsync(int id)
        {
            var products = await _productsService.GetAllProductsByShopAsync(id);
            return Ok(products);
        }

        [HttpPost]
        public async Task<ActionResult> AddAsync(ProductViewModel product)
        {
            await _productsService.SaveAsync(product);
            return NoContent();
        }

        [HttpPut]
        public async Task<ActionResult> EditAsync(ProductViewModel product)
        {
            await _productsService.SaveAsync(product);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteAsync(int id)
        {
            await _productsService.DeleteAsync(id);
            return NoContent();
        }
    }
}