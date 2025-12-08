using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.Marshalling;
using System.Text;
using System.Threading.Tasks;

namespace Hemmuppgiftcrud
{
    public static class DbSeeder
    {
        public static async Task SeedAsync()
        {
            using var db = new ShopContext();

            // make sure the database + migrations have run
            await db.Database.MigrateAsync();

            if (!await db.Customers.AnyAsync())
            {
                db.Customers.AddRange(

                    new Customer { Name = "Gilbert", Email = "Gilbert@hotmail.com", City = "Stockholm" },
                    new Customer { Name = "Anna", Email = "Albert@hotmail.com", City = "Göteborg" }
                );

                await db.SaveChangesAsync();
                Console.WriteLine("Database seeded");
            }

            if (!await db.Orders.AnyAsync())
            {
                db.Orders.Add(new Order { CustomerId = 1, TotalAmount = 49, OrderDate = DateTime.Now.AddDays(2), Status = "Pending" });

                await db.SaveChangesAsync();

                Console.WriteLine("Orders seeded");
            }


            if (!await db.Products.AnyAsync())
            {
                db.Products.AddRange(
                    new Product { Name = "Bed", Price = 1399 },
                    new Product { Name = "Glowstick", Price = 39 },
                    new Product { Name = "Piano", Price = 2899 }
                );

                await db.SaveChangesAsync();
                Console.WriteLine("Products seeded");
            }
            db.Products.AddRange( //lägg till random varor
                new Product { Name = "Hammer", Price = 49 },
                new Product { Name = "Spike", Price = 19 },
                new Product { Name = "Keyboard", Price = 499 },
                new Product { Name = "Mouse", Price = 199 },
                new Product { Name = "Monitor", Price = 1499 },
                new Product { Name = "Desk", Price = 2999 },
                new Product { Name = "Chair", Price = 899 },
                new Product { Name = "USB Cable", Price = 29 },
                new Product { Name = "Webcam", Price = 399 }
            );

            if (!await db.OrderRows.AnyAsync())
            {
                db.OrderRows.Add(new OrderRow { OrderId = 1, Quantity = 1, UnitPrice = 49, ProductId = 1 });

                await db.SaveChangesAsync();
                Console.WriteLine("OrderRows seeded"); 
            }
        }
    }
}
