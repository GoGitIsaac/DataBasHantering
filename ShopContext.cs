using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.Runtime.InteropServices.Marshalling;
using Microsoft.EntityFrameworkCore;
using System.Runtime.CompilerServices;


namespace Hemmuppgiftcrud
{
    
    public class ShopContext : DbContext
    {
        // Maps the data model classes to database tables
        public DbSet<Order> Orders => Set<Order>();
        public DbSet<Product> Products => Set<Product>();
        public DbSet<Customer> Customers => Set<Customer>();
        public DbSet<OrderRow> OrderRows => Set<OrderRow>();

        public DbSet<OrderSummary> OrderSummaries => Set<OrderSummary>();

        public DbSet<CustomerOrderCount> CustomerOrderCounts => Set<CustomerOrderCount>();
        public DbSet<ProductSalesView> ProductSalesViews => Set<ProductSalesView>();

        // I need a database provider, hence below
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            // You create a path/space for the database file to live/go to
            var dbPath = Path.Combine(AppContext.BaseDirectory, "shop.db");
            // Then create the database provider.
            optionsBuilder.UseSqlite($"Filename={dbPath}");
        }
        protected override void OnModelCreating(ModelBuilder modelbuilder)
        {
            modelbuilder.Entity<ProductSalesView>(e =>
            {
                e.HasNoKey();
                e.ToView("ProductSalesView");
            });

            modelbuilder.Entity<CustomerOrderCount>(e => {


                e.HasNoKey();
                e.ToView("CustomerOrderCountView");
            }


);

            // OrderSummary
            modelbuilder.Entity<OrderSummary>(e =>
            {
                e.HasNoKey(); // Missing PK
                e.ToView("OrderSummaryView"); // Connects the table to SQLITE
            });
            // Customer
            modelbuilder.Entity<Customer>(e =>
            {
                e.HasKey(x => x.CustomerId);
                e.Property(x => x.Name).IsRequired().HasMaxLength(100);
                e.Property(x => x.Email).IsRequired().HasMaxLength(100);
                e.Property(x => x.City).HasMaxLength(100);

                e.HasIndex(x => x.Email).IsUnique();

                e.HasMany(x => x.Orders);

            });

            modelbuilder.Entity<Order>(e =>
            {
                e.HasKey(x => x.OrderId);
                e.Property(x => x.OrderDate).IsRequired();
                e.Property(x => x.Status).IsRequired();
                e.Property(x => x.TotalAmount).IsRequired();

                e.HasOne(x => x.Customer)
                 .WithMany(x => x.Orders)
                 .HasForeignKey(x => x.CustomerId)
                 .OnDelete(DeleteBehavior.Cascade);

            });

            modelbuilder.Entity<OrderRow>(e =>
            {
                e.HasKey(x => x.OrderRowId);

                e.Property(x => x.Quantity).IsRequired();
                e.Property(x => x.UnitPrice).IsRequired();

                // Order 1 - N OrderRows
                e.HasOne(x => x.Order)
                .WithMany(x => x.OrderRows)
                .HasForeignKey(x => x.OrderId)
                .OnDelete(DeleteBehavior.Cascade);

                // Product 1 - M OrderRows
                e.HasOne(x => x.Product)
                .WithMany()
                .HasForeignKey(x => x.ProductId)
                .OnDelete(DeleteBehavior.Restrict);

            });

            modelbuilder.Entity<Product>(e =>
            {
                e.HasKey(x => x.ProductId);

                e.Property(x => x.Name).IsRequired().HasMaxLength(100);
                e.Property(x => x.Price).IsRequired();

            });
        }

    }
}
