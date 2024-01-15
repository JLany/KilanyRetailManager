using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RetailManager.UI.Core.Interfaces
{
    public interface ICartItemDisplayModel
    {
        IProductDisplayModel Product { get; set; }
        int QuantityInCart { get; set; }
        string DisplayName { get; }
    }
}
