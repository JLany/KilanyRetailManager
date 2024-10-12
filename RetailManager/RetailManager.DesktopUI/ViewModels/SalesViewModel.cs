using AutoMapper;
using Caliburn.Micro;
using Microsoft.Extensions.Options;
using RetailManager.DesktopUI.Cart;
using RetailManager.DesktopUI.Models;
using RetailManager.DesktopUI.UiEvents;
using RetailManager.UI.Core.Commands;
using RetailManager.UI.Core.Configuration;
using RetailManager.UI.Core.Exceptions;
using RetailManager.UI.Core.Interfaces;
using RetailManager.UI.Core.Models;
using System.ComponentModel;
using System.Windows;

namespace RetailManager.DesktopUI.ViewModels
{
    public class SalesViewModel : Screen
    {
        private readonly IEventAggregator _events;
        private readonly IProductService _productService;
        private readonly ISaleService _saleService;
        private readonly IMapper _mapper;
        private readonly IConfiguration _config;
        private readonly BusinessSettings _business;
        private BindingList<ListedProductDisplayModel> _products = new BindingList<ListedProductDisplayModel>();
        private BindingList<ICartItemDisplayModel> _cart = new BindingList<ICartItemDisplayModel>();

        private ListedProductDisplayModel _selectedProduct;
        private CartItemDisplayModel _selectedCartItem;
        private int _itemQuantity;
        private bool _isCheckingOut;

        public SalesViewModel(
            IEventAggregator events,
            IProductService productService,
            ISaleService saleService,
            IMapper mapper,
            IConfiguration config,
            IOptions<BusinessSettings> business)
        {
            _events = events;
            _productService = productService;
            _saleService = saleService;
            _mapper = mapper;
            _config = config;
            _business = business.Value;
        }

        // Here we override OnInitialize rather that on activate to support going back 
        // to where were you left off things in the page.
        // Use this page with caution. Make sure to ask for a new instance from IoC 
        // when the situation holds for that.
        protected async override Task OnInitializeAsync(CancellationToken cancellationToken)
        {
            await base.OnInitializeAsync(cancellationToken);

            try
            {
                await InitializeFormAsync();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "An error occurred"
                    , MessageBoxButton.OK, MessageBoxImage.Error);

                await TryCloseAsync();
            }
        }

        private async Task InitializeFormAsync()
        {
            try
            {
                Products = new BindingList<ListedProductDisplayModel>(
                    (await LoadProductsAsync())
                    .ToList());
            }
            catch (UnauthorizedException)
            {
                MessageBox.Show("Your session has expired", "An error occurred"
                    , MessageBoxButton.OK, MessageBoxImage.Error);

                await _events.PublishOnUIThreadAsync(new LogoutUiEvent());

                throw;
            }

            Cart = new BindingList<ICartItemDisplayModel>();
            ItemQuantity = 1;

            NotifyOfPropertyChange(() => SubTotal);
            NotifyOfPropertyChange(() => Tax);
            NotifyOfPropertyChange(() => Total);
        }

        private async Task<IEnumerable<ListedProductDisplayModel>> LoadProductsAsync()
        {
            var products = await _productService.GetAllProductsAsync();

            // Map from ListedProductViewModel to DisplayModel.
            return _mapper
                .Map<IEnumerable<ListedProductDisplayModel>>(products);
        }

        #region UI Properties

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

        public BindingList<ICartItemDisplayModel> Cart
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

        #endregion

        public void AddToCart()
        {
            new AddToCartCommand(new BindingListCart(Cart), SelectedProduct, ItemQuantity)
                .Execute();

            ItemQuantity = 1;

            NotifyOfPropertyChange(() => SubTotal);
            NotifyOfPropertyChange(() => Tax);
            NotifyOfPropertyChange(() => Total);
            NotifyOfPropertyChange(() => CanCheckout);
        }

        public void RemoveFromCart()
        {
            new RemoveFromCartCommand(new BindingListCart(Cart), SelectedCartItem)
                .Execute();

            NotifyOfPropertyChange(() => SubTotal);
            NotifyOfPropertyChange(() => Tax);
            NotifyOfPropertyChange(() => Total);
            NotifyOfPropertyChange(() => CanCheckout);
            NotifyOfPropertyChange(() => CanAddToCart);
        }

        public async Task Checkout()
        {
            IsCheckingOut = true;

            try
            {
                await new CheckoutCommand(new BindingListCart(Cart), _saleService)
                    .Execute();

                await InitializeFormAsync();
            }
            catch (UnauthorizedException)
            {
                MessageBox.Show("Your session has expired", "An error occurred"
                    , MessageBoxButton.OK, MessageBoxImage.Error);

                await _events.PublishOnUIThreadAsync(new LogoutUiEvent());
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

        private decimal CalculateSubTotal()
        {
            return
                Cart.Aggregate(0M,
                (sum, item) => sum + item.Product.RetailPrice * item.QuantityInCart
                );
        }

        private decimal CalculateTax()
        {
            decimal taxRate = decimal.Divide(_business.TaxRate, 100);
            decimal taxAmount = Cart
                .Where(item => item.Product.IsTaxable)
                .Aggregate(0M,
                (sum, item) => sum + item.Product.RetailPrice * item.QuantityInCart * taxRate
                );

            return taxAmount;
        }
    }
}
