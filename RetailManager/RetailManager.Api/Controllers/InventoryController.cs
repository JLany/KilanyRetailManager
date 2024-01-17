using RetailManager.Core.Data.Dtos;
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
    public class InventoryController : ApiController
    {
        private readonly IInventoryBatchRepository _inventoryBatchRepository;

        public InventoryController(IInventoryBatchRepository inventoryBatchRepository)
        {
            _inventoryBatchRepository = inventoryBatchRepository;
        }

        [HttpGet]
        [Authorize(Roles = "Manager,Admin")]
        public async Task<IEnumerable<InventoryBatch>> GetAll()
        {
            var batches = await _inventoryBatchRepository.GetAllAsync();

            return batches;
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IHttpActionResult> Create(InventoryBatchDto batchDto)
        {
            var inventoryBatch = new InventoryBatch
            {
                ProductName = batchDto.ProductName,
                ProductId = batchDto.ProductId,
                Quantity = batchDto.Quantity,
                PurchasePrice = batchDto.PurchasePrice,
                PurchaseDate = batchDto.PurchaseDate,
            };

            await _inventoryBatchRepository.AddAsync(inventoryBatch);

            return Created("", inventoryBatch);
        }
    }
}
