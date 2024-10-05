using RetailManager.DesktopUI.Models;
using RetailManager.UI.Core.Interfaces;
using System.Collections;
using System.ComponentModel;

namespace RetailManager.DesktopUI.Cart
{
    public class BindingListCart : ICart
    {
        private readonly BindingList<ICartItemDisplayModel> _cart;

        public BindingListCart(BindingList<ICartItemDisplayModel> list)
        {
            _cart = list;
        }

        public void Add(IProductDisplayModel item, int quantity)
        {
            var candidateItem = _cart
                .FirstOrDefault(cartItem => cartItem.Product.Id == item.Id);

            if (candidateItem is null)
            {
                candidateItem = new CartItemDisplayModel
                {
                    Product = item
                };

                _cart.Add(candidateItem);
            }

            candidateItem.QuantityInCart += quantity;
            item.QuantityInStock -= quantity;
        }

        public IEnumerator<ICartItemDisplayModel> GetEnumerator()
        {
            return _cart.GetEnumerator();
        }

        public void Remove(ICartItemDisplayModel item)
        {
            _cart.Remove(item);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
