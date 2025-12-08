using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hemmuppgiftcrud
{
    /*
     * Detta är en keylless entitet (INGEN PK) 
     * Den representerar en SQL View, en spara SELECT-query
     * Vi använder dessa Views I en EF Core som gör att den kan läsa den precis som en vanlig tabell
     */
    [Keyless] // frivillig. ingen påverkan, påverkar bara att EF inte förväntar sig en primary key
    public class OrderSummary
    {
        public int OrderId { get; set; }
        public DateOnly OrderDate { get; set; }
        public string CustomerOrder { get; set; } = string.Empty;
        public string CustomerEmail { get; set; } = string.Empty;
        public decimal TotalAmount { get; set; }

    }
}
