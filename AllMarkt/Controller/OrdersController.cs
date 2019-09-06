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
    public class OrdersController : ControllerBase
    {
        private readonly IOrdersService _ordersService;
        private readonly IClaimsGetter _claimsGetter;
        private readonly IUsersService _usersService;

        public OrdersController(IOrdersService ordersService, IClaimsGetter claimsGetter, IUsersService usersService)
        {
            _ordersService = ordersService;
            _claimsGetter = claimsGetter;
            _usersService = usersService;
        }

        [AllowAnonymous]
        [HttpGet()]
        public async Task<ActionResult> GetAllAsync()
        {
            var orders = await _ordersService.GetAllAsync();
            return Ok(orders);
        }

        [AllowAnonymous]
        [HttpGet("getShopOrders")]
        public async Task<ActionResult> GetShopOrdersAsync()
        {
            var userId = _claimsGetter.UserId(User?.Claims);
            var shop = await _usersService.GetShopByUserIdAsync(userId);
            var shopId = shop.Id;
            var orders = await _ordersService.GetShopOrdersAsync(shopId);
            return Ok(orders);
        }

        [AllowAnonymous]
        [HttpGet("getCustomerOrders")]
        public async Task<ActionResult> GetCustomerOrdersAsync()
        {
            var userId = _claimsGetter.UserId(User?.Claims);
            var customerId = (await _usersService.GetCustomerByUserIdAsync(userId)).Id;
            var orders = await _ordersService.GetCustomerOrdersAsync(customerId);
            return Ok(orders);
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<ActionResult> AddAsync(OrderViewModel order)
        {
            await _ordersService.SaveAsync(order);
            return NoContent();
        }

        [AllowAnonymous]
        [HttpPut]
        public async Task<ActionResult> EditAsync(OrderViewModel order)
        {
            await _ordersService.SaveAsync(order);
            return NoContent();
        }

        [AllowAnonymous]
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteAsync(int id)
        {
            await _ordersService.DeleteAsync(id);
            return NoContent();
        }
    }
}
