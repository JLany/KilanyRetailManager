﻿using Microsoft.AspNet.Identity;
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
    public class SalesController : ApiController
    {
        private readonly ISaleRepository _saleRepo;
        private readonly ISalePersistence _salePersistence;

        public SalesController(ISaleRepository saleRepo, ISalePersistence salePersistence)
        {
            _saleRepo = saleRepo;
            _salePersistence = salePersistence;
        }

        [HttpGet]
        [Route("Summaries")]
        public async Task<IEnumerable<SaleSummary>> Summaries()
        {
            var summaries = await _saleRepo.GetSaleSummariesAsync();

            return summaries;
        }

        [HttpPost]
        public async Task<IHttpActionResult> Create(SaleDto saleDto)
        {
            Sale sale = await _salePersistence.Create(saleDto, User.Identity.GetUserId());
            
            return Created("", sale);
        }
    }
}
