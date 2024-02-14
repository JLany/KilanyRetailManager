using Microsoft.AspNet.Identity;
using RetailManager.Core.Data.Dtos;
using RetailManager.Core.Data.Models;
using RetailManager.Core.Interfaces;
using Swashbuckle.Swagger;
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
    [RoutePrefix("api/Sales")]
    public class SalesController : ApiController
    {
        private readonly ISaleRepository _saleRepository;
        private readonly ISalePersistence _salePersistence;

        public SalesController(ISaleRepository saleRepo, ISalePersistence salePersistence)
        {
            _saleRepository = saleRepo;
            _salePersistence = salePersistence;
        }

        [HttpGet]
        [Route("Summaries")]
        [Authorize(Roles = "Manager,Admin")]
        public async Task<IEnumerable<SaleSummary>> Summaries()
        {
            var summaries = await _saleRepository.GetSaleSummariesAsync();

            return summaries;
        }

        [HttpPost]
        [Authorize(Roles = "Cashier")]
        public async Task<IHttpActionResult> Create(SaleDto saleDto)
        {
            Sale sale = await _salePersistence.Create(saleDto, User.Identity.GetUserId());
            
            return Created("", sale);
        }
    }
}
