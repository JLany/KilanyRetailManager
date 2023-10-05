using Caliburn.Micro;
using RetailManager.UI.Core.ApiClients;
using RetailManager.UI.Core.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace RetailManager.DesktopUI.ViewModels
{
    public class SalesViewModel : Screen
    {
        private readonly IProductService _productService;

        private BindingList<ListedProductViewModel> _products = new BindingList<ListedProductViewModel>();
        private BindingList<CartItemModel> _cart = new BindingList<CartItemModel>();

        private int _itemQuantity = 1;
        private string _subTotal;
        private string _tax;
        private string _total;
        private ListedProductViewModel _selectedProduct;

        public SalesViewModel(IProductService productService)
        {
            _productService = productService;
        }

        protected async override Task OnActivateAsync(CancellationToken cancellationToken)
        {
            await base.OnActivateAsync(cancellationToken);

            var products = await _productService.GetProductsAsync();
            Products = new BindingList<ListedProductViewModel>(products.ToList());
        }

        public BindingList<ListedProductViewModel> Products
        {
            get => _products;
            set
            {
                _products = value;
                NotifyOfPropertyChange(() => Products);
            }
        }

        public BindingList<CartItemModel> Cart
        {
            get => _cart;
            set
            {
                _cart = value;
                NotifyOfPropertyChange(() => Cart);
            }
        }

        public int ItemQuantity
        {
            get => _itemQuantity;   
            set
            {
                _itemQuantity = value;
                NotifyOfPropertyChange(() => ItemQuantity);
                NotifyOfPropertyChange(() => CanAddToCart);
            }
        }

        public string SubTotal
        {
            get
            {
                // Calculate the actual amount.
                var total = Cart
                    .Aggregate(0M,
                    (t, cartItem) => t + cartItem.Product.RetailPrice * cartItem.QuantityInCart
                    );

                //return $"{total:C}";
                return total.ToString("C");
            }
        }

        public string Tax
        {
            get
            {
                // Calculate the actual amount.

                return "$0.00";
            }
        }

        public string Total
        {
            get
            {
                // Calculate the actual amount.

                return "$0.00";
            }
        }

        public bool CanAddToCart
        {
            get
            {
                if (SelectedProduct is null)
                {
                    return false;
                }

                if (ItemQuantity < 1 || ItemQuantity > SelectedProduct.QuantityInStock)
                {
                    return false;
                }

                return true;
            }
        }

        public bool CanRemoveFromCart
        {
            get
            {
                // Make sure there is item selected.

                return false;
            }
        }

        public bool CanCheckout
        {
            get
            {
                if (Cart.Count < 1)
                {
                    return false;
                }

                return true;
            }
        }

        public ListedProductViewModel SelectedProduct 
        { 
            get => _selectedProduct;
            set
            {
                _selectedProduct = value;
                NotifyOfPropertyChange(() => SelectedProduct);
                NotifyOfPropertyChange(() => CanAddToCart);
            } 
        }

        public void AddToCart()
        {
            var candidateItem = Cart
                .FirstOrDefault(cartItem => cartItem.Product.Id == SelectedProduct.Id);

            if (candidateItem is null)
            {
                candidateItem = new CartItemModel
                {
                    Product = SelectedProduct
                };

                Cart.Add(candidateItem);
            }

            candidateItem.QuantityInCart += ItemQuantity;
            SelectedProduct.QuantityInStock -= ItemQuantity;                
            ItemQuantity = 1;

            // HACK - There should be a better way to do this refreshing.
            {
                Cart.Remove(candidateItem);
                Cart.Add(candidateItem);
            }

            NotifyOfPropertyChange(() => SubTotal);
            NotifyOfPropertyChange(() => CanCheckout);
        }

        public void RemoveFromCart()
        {

        }

        public void Checkout()
        {

        }
    }
}
