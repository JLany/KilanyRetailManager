using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RetailManager.UI.Core.Interfaces
{
    public interface ICart : IEnumerable<ICartItemDisplayModel>
    {
        void Add(IProductDisplayModel item, int quantity);
        void Remove(ICartItemDisplayModel item);
    }
}
