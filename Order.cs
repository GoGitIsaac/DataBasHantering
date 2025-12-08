using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hemmuppgiftcrud
{
    public class Order
    {
        //PK
        public int OrderId { get; set; }
        //FK
        public int CustomerId { get; set; }
        [Required]
        public DateTime OrderDate { get; set; }
        [Required, StringLength(50)]
        public string? Status { get; set; }
        [Required]
        public decimal TotalAmount { get; set; }
        public Customer? Customer { get; set; }
        public List<OrderRow>? OrderRows;

    }
}
