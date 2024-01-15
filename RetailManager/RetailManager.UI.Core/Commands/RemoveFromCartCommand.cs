using RetailManager.UI.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RetailManager.UI.Core.Commands
{
    public class RemoveFromCartCommand : ICommand
    {
        private readonly ICart _cart;
        private readonly ICartItemDisplayModel _cartItemToRemove;

        public RemoveFromCartCommand(ICart cart, ICartItemDisplayModel cartItemToRemove)
        {
            _cartItemToRemove = cartItemToRemove;
            _cart = cart;
        }

        public Task Execute()
        {
            _cartItemToRemove.Product.QuantityInStock += 1;
            _cartItemToRemove.QuantityInCart -= 1;

            if (_cartItemToRemove.QuantityInCart < 1)
            {
                _cart.Remove(_cartItemToRemove);
            }

            return Task.CompletedTask;
        }
    }
}
