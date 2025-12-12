using Hemmuppgiftcrud;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.Unicode;
using System.Threading.Tasks;

namespace Hemmuppgiftcrud
{
    public class CustomerService
    {
        private async Task SaveAsync(List<Customer> customers)
        {
            var toSave = customers.Select(c =>
            {
                // Hash Nameif it exists but is missing salt/hash
                if (!string.IsNullOrEmpty(c.Name) &&
                    string.IsNullOrEmpty(c.CustomerNameSalt))
                {
                    var salt = HashingHelper.GenerateSalt();
                    var hash = HashingHelper.HashWithSalt(c.Name, salt);

                    c.CustomerNameSalt = salt;
                    c.CustomerNameHash = hash;
                }

                return new Customer
                {
                    CustomerId = c.CustomerId,


                    // Don't save Name directly, save only the hash + salt
                    Name = null,
                    City = c.City,
                    Email = c.Email,
                    CustomerNameHash = c.CustomerNameHash,
                    CustomerNameSalt = c.CustomerNameSalt
                };
            });
        }
    }
}



