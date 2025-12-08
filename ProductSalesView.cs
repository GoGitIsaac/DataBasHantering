using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hemmuppgiftcrud
{
    [Keyless]
    public class ProductSalesView
    {
        public int ProductId { get; set; }
        public string Name { get; set; } = string.Empty;
        public int TotalQuantitySold { get; set; }
    }
}
