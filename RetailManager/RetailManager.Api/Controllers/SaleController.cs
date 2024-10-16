﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using RetailManager.Api.Data.Entities;
using RetailManager.Core.Data.Dtos;
using RetailManager.Core.Data.Models;
using RetailManager.Core.Interfaces;

namespace RetailManager.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class SaleController : ControllerBase
    {
        private readonly ISaleRepository _saleRepository;
        private readonly ISalePersistence _salePersistence;
        private readonly UserManager<RetailManagerAuthUser> _userManager;

        public SaleController(ISaleRepository saleRepo, ISalePersistence salePersistence, UserManager<RetailManagerAuthUser> userManager)
        {
            _saleRepository = saleRepo;
            _salePersistence = salePersistence;
            _userManager = userManager;
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
        public async Task<IActionResult> Create(SaleRequest saleDto)
        {
            var userId = _userManager.GetUserId(User);
            Sale sale = await _salePersistence.Create(saleDto, userId);

            return Created("", sale);
        }
    }
}
