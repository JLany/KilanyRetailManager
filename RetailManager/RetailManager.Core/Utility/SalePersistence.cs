using Microsoft.Extensions.Options;
using RetailManager.Core.Configuration;
using RetailManager.Core.Data.Dtos;
using RetailManager.Core.Data.Models;
using RetailManager.Core.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RetailManager.Core.Utility
{
    /// <summary>
    /// A Facade to the <see cref="Sale"/> model creation subsystem of operations.
    /// </summary>
    public class SalePersistence : ISalePersistence
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ISaleRepository _saleRepository;
        private readonly ISaleDetailRepository _saleDetailRepository;
        private readonly IProductRepository _productRepository;
        private readonly IConfiguration _config;
        private readonly TaxSettings _taxSettings;

        public SalePersistence(
            IUnitOfWork unitOfWork,
            ISaleRepository saleRepository,
            ISaleDetailRepository saleDetailRepository,
            IProductRepository productRepository,
            IConfiguration config,
            IOptions<TaxSettings> taxSettings)
        {
            _unitOfWork = unitOfWork;
            _saleRepository = saleRepository;
            _saleDetailRepository = saleDetailRepository;
            _productRepository = productRepository;
            _config = config;
            _taxSettings = taxSettings.Value;
        }

        /// <summary>
        /// Create a <see cref="Sale"/> model in the database.
        /// </summary>
        /// <param name="saleDto"></param>
        /// <param name="cashierId"></param>
        /// <returns>The created <see cref="Sale"/> model in the database.</returns>
        public async Task<Sale> Create(SaleRequest saleDto, string cashierId)
        {
            Sale sale = new Sale
            {
                CashierId = cashierId
            };

            try
            {
                _unitOfWork.Begin();

                await _saleRepository.AddAsync(sale);

                IEnumerable<SaleDetail> saleDetails =
                    saleDto.SaleDetails.Select(sd => new SaleDetail
                    {
                        SaleId = sale.Id,
                        ProductId = sd.ProductId,
                        Quantity = sd.Quantity,
                    });

                var taxRate = decimal.Divide(_taxSettings.TaxRate, 100);
                decimal subTotal = 0M;
                decimal taxAmount = 0M;

                foreach (var d in saleDetails)
                {
                    Product product = await _productRepository.GetAsync(d.ProductId);

                    d.PurchasePrice = product.RetailPrice;
                    subTotal += d.PurchasePrice;

                    if (product.IsTaxable)
                    {
                        d.Tax = d.PurchasePrice * d.Quantity * taxRate;
                        taxAmount += d.Tax;
                    }

                    await _saleDetailRepository.AddAsync(d);
                }

                sale.SubTotal = subTotal;
                sale.Tax = taxAmount;
                sale.Total = sale.SubTotal + sale.Tax;

                await _saleRepository.UpdateAsync(sale);

                _unitOfWork.Commit();
            }
            catch
            {
                _unitOfWork.Rollback();
                throw;
            }


            return sale;
        }
    }
}
