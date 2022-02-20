using System;
using System.Collections.Generic;

#nullable disable

namespace IndacoProject.Corso.Data.Entities.Northwind
{
    public partial class ProductsAboveAveragePrice
    {
        public string ProductName { get; set; }
        public decimal? UnitPrice { get; set; }
    }
}
