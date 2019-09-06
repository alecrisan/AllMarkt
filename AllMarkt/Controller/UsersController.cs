using AllMarkt.Services.Interfaces;
using AllMarkt.Tools;
using AllMarkt.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;

namespace AllMarkt.Controller
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly IUsersService _usersService;
        private readonly IClaimsGetter _claimsGetter;

        public UsersController(IUsersService usersService, IClaimsGetter claimsGetter)
        {
            _usersService = usersService;
            _claimsGetter = claimsGetter;
        }

        [AllowAnonymous]
        [HttpGet("shops")]
        public async Task<ActionResult> GetAllShopsAsync()
        {
            var shops = await _usersService.GetAllShopsAsync();
            return Ok(shops);
        }

        [AllowAnonymous]
        [HttpGet("shopCategoryId={id}")]
        public async Task<ActionResult> GetShopsByCategoryIdAsync(int id)
        {
            var shopsByCategoryId = await _usersService.GetShopsByCategoryIdAsync(id);
            return Ok(shopsByCategoryId);
        }

        [AllowAnonymous]
        [HttpGet("customers")]
        public async Task<ActionResult> GetAllCustomersAsync()
        {
            var customers = await _usersService.GetAllCustomersAsync();
            return Ok(customers);
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<ActionResult> GetAllAsync()
        {
            var users = await _usersService.GetAllAsync();
            return Ok(users);
        }

        [AllowAnonymous]
	    [HttpGet("byId")]
	    //[HttpGet("{id}")]
        public async Task<ActionResult> GetByIdAsync()
        {
            var userId = _claimsGetter.UserId(User?.Claims);
            var userById = await _usersService.GetByIdAsync(userId);
            return Ok(userById);
        }

        [AllowAnonymous]
        [HttpGet("customers/byUser")]
        public async Task<ActionResult> GetCustomerByUserIdAsync()
        {
            var userId = _claimsGetter.UserId(User?.Claims);
            var customerById = await _usersService.GetCustomerByUserIdAsync(userId);
            return Ok(customerById);
        }

        [AllowAnonymous]
        [HttpGet("customers/byId/{id}")]
        public async Task<ActionResult> GetCustomerByIdAsync(int id)
        {
            var customerById = await _usersService.GetCustomerByIdAsync(id);
            return Ok(customerById);
        }

        [AllowAnonymous]
        [HttpGet("shops/byUser")]
        public async Task<ActionResult> GetShopByUserIdAsync()
        {
            var userId = _claimsGetter.UserId(User?.Claims);
            var shopById = await _usersService.GetShopByUserIdAsync(userId);
            return Ok(shopById);
        }

        [AllowAnonymous]
        [HttpGet("shops/byId/{id}")]
        public async Task<ActionResult> GetShopByIdAsync(int id)
        {
            var shopById = await _usersService.GetShopByIdAsync(id);
            return Ok(shopById);
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<ActionResult> AddAsync(UserInputViewModel userInputViewModel)
        {
            await _usersService.SaveAsync(userInputViewModel);
            return NoContent();
        }

        [AllowAnonymous]
        [HttpPost("newUser")]
        public async Task<ActionResult> RegisterAsync(UserInputViewModel userInputViewModel)
        {
            await _usersService.RegisterAsync(userInputViewModel);
            return NoContent();
        }
        [HttpPut]
        public async Task<ActionResult> EditAsync(UserEditViewModel userViewModel)
        {
            await _usersService.EditAsync(userViewModel);
            return NoContent();
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteAsync(int id)
        {
            await _usersService.DeleteAsync(id);
            return NoContent();
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<IActionResult> Authenticate(UserLoginRequestViewModel userParam)
        {
            var user = await _usersService.Authenticate(userParam.Email, userParam.Password);

            if (user == null)
            {
                return BadRequest(new { message = "Email or password incorrect" });
            }

            return Ok(user);
        }

        [AllowAnonymous]
        [HttpPut("shops")]
        public async Task<ActionResult> EditShopAsync(ShopViewModel shopViewModel)
        {
            await _usersService.EditShopAsync(shopViewModel);
            return NoContent();
        }

        [HttpPost("isTokenExpired")]
        public ActionResult IsTokenExpired()
        {
            return NoContent();
        }
        
        [AllowAnonymous]
        [HttpPut("customers")]
        public async Task<ActionResult> EditCustomerAsync(CustomerViewModel customerViewModel)
        {
            await _usersService.EditCustomerAsync(customerViewModel);
            return NoContent();
        }

        [HttpGet("getMyDataAsNormalUser")]
        public async Task<ActionResult> GetMyData()
        {
            var userData = await _usersService.GetByIdAsync(_claimsGetter.UserId(User?.Claims));
            return Ok(userData);
        }

        [HttpGet("getMyDataAsShop")]
        public async Task<ActionResult> GetMyDataAsShop()
        {
            var userData = await _usersService.GetShopByUserIdAsync(_claimsGetter.UserId(User?.Claims));
            return Ok(userData);
        }

        [HttpGet("getMyDataAsCustomer")]
        public async Task<ActionResult> GetMyDataAsCustomer()
        {
            var userData = await _usersService.GetCustomerByUserIdAsync(_claimsGetter.UserId(User?.Claims));
            return Ok(userData);
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("disableUser")]
        public async Task<ActionResult> DisableUserByIdAsync(UserEditViewModel userEditViewModel)
        {
            await _usersService.DisableUserByIdAsync(userEditViewModel);
            return NoContent();
        }

        protected int GetUserId()
        {
            return int.Parse(this.User.Claims.First(i => i.Type == "userId").Value);
        }
    }
}
