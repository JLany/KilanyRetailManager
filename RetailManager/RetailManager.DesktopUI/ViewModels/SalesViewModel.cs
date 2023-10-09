using AutoMapper;
using Caliburn.Micro;
using RetailManager.DesktopUI.Models;
using RetailManager.UI.Core.Dtos;
using RetailManager.UI.Core.Interfaces;
using RetailManager.UI.Core.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace RetailManager.DesktopUI.ViewModels
{
    public class SalesViewModel : Screen
    {
        private readonly IProductService _productService;
        private readonly ISaleService _saleService;
        private readonly IMapper _mapper;
        private readonly IConfiguration _config;
        private BindingList<ListedProductDisplayModel> _products = new BindingList<ListedProductDisplayModel>();
        private BindingList<CartItemDisplayModel> _cart = new BindingList<CartItemDisplayModel>();

        private ListedProductDisplayModel _selectedProduct;
        private CartItemDisplayModel _selectedCartItem;
        private int _itemQuantity;
        private bool _isCheckingOut;

        public SalesViewModel(
            IProductService productService,
            ISaleService saleService,
            IMapper mapper,
            IConfiguration config)
        {
            _productService = productService;
            _saleService = saleService;
            _mapper = mapper;
            _config = config;
        }

        protected async override Task OnActivateAsync(CancellationToken cancellationToken)
        {
            await base.OnActivateAsync(cancellationToken);

            await InitializeFormAsync();
        }

        private async Task InitializeFormAsync()
        {
            Products = new BindingList<ListedProductDisplayModel>((await LoadProductsAsync()).ToList());
            Cart = new BindingList<CartItemDisplayModel>();

            ItemQuantity = 1;

            NotifyOfPropertyChange(() => SubTotal);
            NotifyOfPropertyChange(() => Tax);
            NotifyOfPropertyChange(() => Total);
        }

        public BindingList<ListedProductDisplayModel> Products
        {
            get => _products;
            set
            {
                _products = value;
                NotifyOfPropertyChange(() => Products);
                NotifyOfPropertyChange(() => CanAddToCart);
            }
        }

        public BindingList<CartItemDisplayModel> Cart
        {
            get => _cart;
            set
            {
                _cart = value;
                NotifyOfPropertyChange(() => Cart);
                NotifyOfPropertyChange(() => CanRemoveFromCart);
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

        public bool IsCheckingOut
        {
            get => _isCheckingOut;
            set
            {
                _isCheckingOut = value;
                NotifyOfPropertyChange(() => IsCheckingOut);
                NotifyOfPropertyChange(() => CanCheckout);
            }
        }

        public string SubTotal =>
                //return $"{total:C}";
                CalculateSubTotal().ToString("C");

        public string Tax => CalculateTax().ToString("C");

        public string Total => (CalculateSubTotal() + CalculateTax()).ToString("C");

        public bool CanAddToCart
        {
            get
            {
                if (SelectedProduct is null)
                {
                    return false;
                }

                if (SelectedProduct.QuantityInStock < ItemQuantity || ItemQuantity < 1)
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
                if (SelectedCartItem is null)
                {
                    return false;
                }

                if (SelectedCartItem.QuantityInCart < 1)
                {
                    return false;
                }

                return true;
            }
        }

        public bool CanCheckout => Cart.Any() && !IsCheckingOut;

        public ListedProductDisplayModel SelectedProduct 
        { 
            get => _selectedProduct;
            set
            {
                _selectedProduct = value;
                NotifyOfPropertyChange(() => SelectedProduct);
                NotifyOfPropertyChange(() => CanAddToCart);
            } 
        }

        public CartItemDisplayModel SelectedCartItem
        {
            get => _selectedCartItem;
            set
            {
                _selectedCartItem = value;
                NotifyOfPropertyChange(() => SelectedCartItem);
                NotifyOfPropertyChange(() => CanRemoveFromCart);
            }
        }

        public void AddToCart()
        {
            var candidateItem = Cart
                .FirstOrDefault(cartItem => cartItem.Product.Id == SelectedProduct.Id);

            if (candidateItem is null)
            {
                candidateItem = new CartItemDisplayModel
                {
                    Product = SelectedProduct
                };

                Cart.Add(candidateItem);
            }

            candidateItem.QuantityInCart += ItemQuantity;
            SelectedProduct.QuantityInStock -= ItemQuantity;                
            ItemQuantity = 1;

            NotifyOfPropertyChange(() => SubTotal);
            NotifyOfPropertyChange(() => Tax);
            NotifyOfPropertyChange(() => Total);
            NotifyOfPropertyChange(() => CanCheckout);
        }

        public void RemoveFromCart()
        {
            SelectedCartItem.Product.QuantityInStock += 1;
            SelectedCartItem.QuantityInCart -= 1;

            if (SelectedCartItem.QuantityInCart < 1)
            {
                Cart.Remove(SelectedCartItem);
            }

            NotifyOfPropertyChange(() => SubTotal);
            NotifyOfPropertyChange(() => Tax);
            NotifyOfPropertyChange(() => Total);
            NotifyOfPropertyChange(() => CanCheckout); 
            NotifyOfPropertyChange(() => CanAddToCart);
        }

        public async Task Checkout()
        {
            IsCheckingOut = true;

            // ELEGANCE.
            SaleDto saleDto = new SaleDto
            {
                SaleDetails = Cart.Select(
                    item => new SaleDetailDto
                    {
                        ProductId = item.Product.Id,
                        Quantity = item.QuantityInCart
                    })
            };

            try
            {
                await _saleService.PostSaleAsync(saleDto);

                await InitializeFormAsync();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "An error occurred"
                    , MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                IsCheckingOut = false;
            }
        }

        private async Task<IEnumerable<ListedProductDisplayModel>> LoadProductsAsync()
        {
            var products = await _productService.GetProductsAsync();

            // Map from ListedProductViewModel to DisplayModel.
            return _mapper
                .Map<IEnumerable<ListedProductDisplayModel>>(products);
        }

        private decimal CalculateSubTotal()
        {
            return
                Cart.Aggregate(0M,
                (sum, item) => sum + item.Product.RetailPrice * item.QuantityInCart
                );
        }

        private decimal CalculateTax()
        {
            decimal taxRate = decimal.Divide(_config.GetTaxRate(), 100);
            decimal taxAmount = Cart
                .Where(item => item.Product.IsTaxable)
                .Aggregate(0M,
                (sum, item) => sum + item.Product.RetailPrice * item.QuantityInCart * taxRate
                );

            return taxAmount;
        }
    }
}
