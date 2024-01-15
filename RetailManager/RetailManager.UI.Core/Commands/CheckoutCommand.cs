using RetailManager.UI.Core.Dtos;
using RetailManager.UI.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RetailManager.UI.Core.Commands
{
    public class CheckoutCommand : ICommand
    {
        private readonly ICart _cart;
        private readonly ISaleService _saleService;

        public CheckoutCommand(ICart cart, ISaleService saleService)
        {
            _cart = cart;
            _saleService = saleService;
        }

        public async Task Execute()
        {
            SaleDto saleDto = new SaleDto
            {
                SaleDetails = _cart.Select(
                    item => new SaleDetailDto
                    {
                        ProductId = item.Product.Id,
                        Quantity = item.QuantityInCart
                    })
            };

            await _saleService.PostSaleAsync(saleDto);
        }
    }
}
