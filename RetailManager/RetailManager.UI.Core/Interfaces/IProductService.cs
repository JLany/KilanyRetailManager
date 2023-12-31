﻿using RetailManager.UI.Core.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RetailManager.UI.Core.Interfaces
{
    public interface IProductService
    {
        Task<IEnumerable<ListedProductViewModel>> GetProductsAsync();
    }
}