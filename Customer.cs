using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hemmuppgiftcrud
{
    public class Customer
    {
        // PK
        public int CustomerId { get; set; }
        [Required, MaxLength(100)]
        public string? Name { get; set; }
        [Required, MaxLength(100)]
        public string? Email { get; set; }
        public string? City { get; set; }

        public List<Order> Orders { get; set; } = new();
    }
}
