using Caliburn.Micro;
using RetailManager.UI.Core.Interfaces;
using RetailManager.UI.Core.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace RetailManager.DesktopUI.Models
{
    public class CartItemDisplayModel : INotifyPropertyChanged, ICartItemDisplayModel
    {
        private int _quantityInCart;

        public IProductDisplayModel Product { get; set; }
        public int QuantityInCart
        {
            get => _quantityInCart;
            set
            {
                _quantityInCart = value;
                NotifyPropertyChange(() => QuantityInCart);
                NotifyPropertyChange(() => DisplayName);
            }
        }
        public string DisplayName => $"{Product.ProductName} ({QuantityInCart})";

        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChange<TResult>(Expression<Func<TResult>> property)
        {
            PropertyChanged?
                .Invoke(this, new PropertyChangedEventArgs(property.GetMemberInfo().Name));
        }
    }
}
