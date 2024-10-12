using RetailManager.UI.Core.Interfaces;
using RetailManager.UI.Core.Models.Dtos;
using System.Linq;
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
                    .ToList()
            };

            await _saleService.CreateSaleAsync(saleDto);
        }
    }
}
