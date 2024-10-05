using RetailManager.Core.Interfaces;
using System.Threading.Tasks;
using System.Web.Http;

namespace RetailManager.Api.Controllers
{
    [Authorize(Roles = "Cashier")]
    public class ProductsController : ApiController
    {
        private readonly IProductRepository _productRepository;

        public ProductsController(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public async Task<IHttpActionResult> Get()
        {
            var products = await _productRepository.GetAllAsync();

            return Ok(products);
        }
    }
}
