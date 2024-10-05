using System.Collections.Generic;

namespace RetailManager.UI.Core.Interfaces
{
    public interface ICart : IEnumerable<ICartItemDisplayModel>
    {
        void Add(IProductDisplayModel item, int quantity);
        void Remove(ICartItemDisplayModel item);
    }
}
