using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RetailManager.Core.Interfaces;

namespace RetailManager.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Cashier")]
    public class ProductController : ControllerBase
    {
        private readonly IProductRepository _productRepository;

        public ProductController(IProductRepository productRepository) 
        {
            _productRepository = productRepository;
        }

        public async Task<IActionResult> Get()
        {
            var products = await _productRepository.GetAllAsync();

            return Ok(products);
        }
    }
}
