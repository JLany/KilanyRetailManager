using RetailManager.Core.Data.Models;
using RetailManager.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace RetailManager.Api.Controllers
{
    [Authorize]
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
