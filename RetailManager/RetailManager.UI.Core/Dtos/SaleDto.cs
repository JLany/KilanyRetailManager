﻿using System.Collections.Generic;

namespace RetailManager.UI.Core.Dtos
{
    public class SaleDto
    {
        public List<SaleDetailDto> SaleDetails { get; set; } = new List<SaleDetailDto>();
    }
}
