using Caliburn.Micro;
using RetailManager.UI.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace RetailManager.DesktopUI.Models
{
    public class ListedProductDisplayModel : INotifyPropertyChanged, IProductDisplayModel
    {
        private int _quantityInStock;

        /// <summary>
        /// Global unique identifier for the product in database.
        /// </summary>
        public int Id { get; set; }
        public string ProductName { get; set; }
        public string Description { get; set; }
        public decimal RetailPrice { get; set; }
        public int QuantityInStock
        {
            get => _quantityInStock;
            set
            {
                _quantityInStock = value;
                NotifyPropertyChange(() => QuantityInStock);
                NotifyPropertyChange(() => DisplayName);
            }
        }
        public bool IsTaxable { get; set; }
        public string DisplayName => $"{ProductName} ({QuantityInStock})";

        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChange<TResult>(Expression<Func<TResult>> property)
        {
            PropertyChanged?
                .Invoke(this, new PropertyChangedEventArgs(property.GetMemberInfo().Name));
        }
    }
}
