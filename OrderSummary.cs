using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hemmuppgiftcrud
{
    /*
     * This is a keyless entity (NO PK) 
     * It represent an SQL view, a saved SELECT-query
     * We use these Views in a EF Core so that it can read it as if it was a normal table. 
     */
    [Keyless] // Optional. No Impact, Only impacts so that EF doesn't expect a PK
    public class OrderSummary
    {
        public int OrderId { get; set; }
        public DateOnly OrderDate { get; set; }
        public string CustomerOrder { get; set; } = string.Empty;
        public string CustomerEmail { get; set; } = string.Empty;
        public decimal TotalAmount { get; set; }

    }
}
