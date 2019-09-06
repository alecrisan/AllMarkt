using AllMarkt.Services.Interfaces;
using AllMarkt.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace AllMarkt.Controller
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class CategoriesController : ControllerBase
    {
        private readonly ICategoriesService _categoriesService;

        public CategoriesController(ICategoriesService categoriesService)
        {
            _categoriesService = categoriesService;
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<ActionResult> GetAllAsync()
        {
            var categories = await _categoriesService.GetAllAsync();
            return Ok(categories);
        }

        [HttpPost]
        public async Task<ActionResult> AddAsync(CategoryViewModel category)
        {
            await _categoriesService.SaveAsync(category);
            return NoContent();
        }

        [HttpPut] 
        public async Task<ActionResult> EditAsync(CategoryViewModel category)
        {
            await _categoriesService.SaveAsync(category);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteAsync(int id)
        {
            await _categoriesService.DeleteAsync(id);
            return NoContent();
        }
    }
}
