using RetailManager.UI.Core.Interfaces;
using System.Threading.Tasks;

namespace RetailManager.UI.Core.Commands
{
    public class AddToCartCommand : ICommand
    {
        private readonly ICart _cart;
        private readonly IProductDisplayModel _prodcutToAdd;
        private readonly int _quantity;

        public AddToCartCommand(ICart cart, IProductDisplayModel product, int quantity)
        {
            _cart = cart;
            _prodcutToAdd = product;
            _quantity = quantity;
        }

        public Task Execute()
        {
            _cart.Add(_prodcutToAdd, _quantity);

            return Task.CompletedTask;
        }
    }
}
