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
        private BindingList<ListedProductViewModel> _cart = new BindingList<ListedProductViewModel>();

        private int _itemQuantity;
        private string _subTotal;
        private string _tax;
        private string _total;

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

        public BindingList<ListedProductViewModel> Cart
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
            }
        }

        public string SubTotal
        {
            get
            {
                // Calculate the actual amount.

                return "$0.00";
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
                // Make sure there is item selected.
                // Make sure there is quantity specified.

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
                // Make sure cart is not empty.

                return true;
            }
        }

        public void AddToCart()
        {

        }

        public void RemoveFromCart()
        {

        }

        public void Checkout()
        {

        }
    }
}
