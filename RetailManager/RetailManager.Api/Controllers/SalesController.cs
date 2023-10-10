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
    public class SalesController : ApiController
    {
        private readonly IConfiguration _config;
        private readonly ISaleRepository _saleRepo;
        private readonly ISaleDetailRepository _saleDetailRepo;
        private readonly IProductRepository _productRepo;
        private readonly IUnitOfWork _unitOfWork;

        public SalesController(
            IConfiguration config,
            ISaleRepository saleRepo,
            ISaleDetailRepository saleDetailRepo,
            IProductRepository productRepo,
            IUnitOfWork unitOfWork)
        {
            _config = config;
            _saleRepo = saleRepo;
            _saleDetailRepo = saleDetailRepo;
            _productRepo = productRepo;
            _unitOfWork = unitOfWork;
        }

        public async Task<IHttpActionResult> Create(SaleDto saleDto)
        {
            Sale sale = new Sale
            {
                CashierId = User.Identity.GetUserId()
            };

            try
            {
                _unitOfWork.Begin();

                await _saleRepo.AddAsync(sale);

                IEnumerable<SaleDetail> saleDetails =
                    saleDto.SaleDetails.Select(sd => new SaleDetail
                    {
                        SaleId = sale.Id,
                        ProductId = sd.ProductId,
                        Quantity = sd.Quantity,
                    });

                var taxRate = decimal.Divide(_config.GetTaxRate(), 100);
                decimal subTotal = 0M;
                decimal taxAmount = 0M;

                foreach (var d in saleDetails)
                {
                    Product product = await _productRepo.GetAsync(d.ProductId);

                    d.PurchasePrice = product.RetailPrice;
                    subTotal += d.PurchasePrice;

                    if (product.IsTaxable)
                    {
                        d.Tax = d.PurchasePrice * d.Quantity * taxRate;
                        taxAmount += d.Tax;
                    }

                    await _saleDetailRepo.AddAsync(d);
                }

                sale.SubTotal = subTotal;
                sale.Tax = taxAmount;
                sale.Total = sale.SubTotal + sale.Tax;

                await _saleRepo.UpdateAsync(sale);

                _unitOfWork.Commit();
            }
            catch 
            {
                _unitOfWork.Rollback();
                throw;
            }
            
            return Created("", sale);
        }
    }
}
