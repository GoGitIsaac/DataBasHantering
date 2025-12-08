using Hemmuppgiftcrud;
using Microsoft.EntityFrameworkCore;
using System.IO;

Console.WriteLine(Path.Combine(AppContext.BaseDirectory, "shop.db"));

await DbSeeder.SeedAsync();

// CLI Menu

await Menu();
static async Task Menu()
{
    while (true)
    {

        Console.WriteLine("\n 1 = CustomerMenu | 2= OrderMenu | 3 = ProductMenu | 4 = Exit | 5 = return");
        string options = Console.ReadLine()?.ToLower() ?? string.Empty;
        switch (options)
        {
            case "1":
                await CustomerMenuAsync();
                break;
            case "2":
                await OrderMenuAsync();
                break;
            case "3":
                await ProductMenuAsync();
                break;
            case "4":
                Console.WriteLine("Exiting...");
                break;
            case "5":

                break;
            default:
                Console.WriteLine("Please select a valid option");
                break;

        }
    }
}

static async Task NumberOfOrdersAsync()
{
    using var db = new ShopContext();
    
    var orders = await db.CustomerOrderCounts
                  .OrderByDescending(c => c.CustomerId)
                  .ToListAsync();

    foreach (var order in orders)
    {
        Console.WriteLine($"CustomerId: {order.CustomerId}  | NumberOfOrders: {order.NumberOfOrders}");
    }
}

static async Task ListCustomerAsync()
{
    using var db = new ShopContext();
    var customers = await db.Customers
                  .AsNoTracking().Include(o => o.Orders)
                  .OrderBy(o => o.CustomerId)
                  .ToListAsync();

    Console.WriteLine("\n Id | Name | City | Email");
    foreach (var customer in customers)
    {
        Console.WriteLine($"{customer.CustomerId} | {customer.Name} | {customer.City} | {customer.Email}");
        var customerOrder = customer.Orders?.Count;
        Console.WriteLine("-------------- ORDERS --------------");
        foreach (var order in customer.Orders)
        {
            Console.WriteLine( $"OrderId: {order.OrderId} | TotalAmount: ${order.TotalAmount}");
            Console.WriteLine();
        }
    }
}

static async Task CustomerMenuAsync()
{
    while (true)
    {

        // Delar upp raden på mellanslag: t.ex "edit 2" --> ["edit", "2"]

        Console.WriteLine("\n 1 = ListCustomers | 2 = AddCustomer | 3 <id> = EditCustomer | 4 <id> = Delete | 5 = Menu");
        Console.WriteLine("> ");

        string customerMenu = Console.ReadLine()?.Trim() ?? string.Empty;
        var parts = customerMenu.Split(' ', StringSplitOptions.RemoveEmptyEntries);

        var cmd = parts[0].ToLowerInvariant();
        switch (cmd)
        {
            case "1":
                await ListCustomerAsync();
                break;
            case "2":
                await AddCustomerAsync();
                break;
            case "3":
                // Update a customer / Change them
                if (parts.Length < 2 || !int.TryParse(parts[1], out var id))
                {
                    Console.WriteLine("Usage: 3 <id>");
                    break;
                }
                await EditCustomerAsync(id);
                break;
            case "4":
                // Delete A Customer
                if (parts.Length < 2 || !int.TryParse(parts[1], out var idD))
                {
                    Console.WriteLine("Usage: 5 <id>");
                    break;
                }
                await DeleteCustomerAsync(idD);
                break;
            case "5":
                await Menu();
                break;
            default:
                Console.WriteLine("Please select a valid option");
                break;

        }
    }
}

static async Task AddCustomerAsync()
{
    Console.WriteLine("Name: ");
    var name = Console.ReadLine()?.Trim() ?? string.Empty;

    // Enkel validering
    if (string.IsNullOrEmpty(name) || name.Length > 100)
    {
        Console.WriteLine("Name is required (max 100 characters).");
        return;
    }

    Console.Write("Email: ");
    var email = Console.ReadLine()?.Trim() ?? string.Empty;

    if (string.IsNullOrEmpty(email) || email.Length > 100)
    {
        Console.WriteLine("Email is required (max 100 characters).");
        return;
    }

    Console.Write("City: ");
    var city = Console.ReadLine()?.Trim() ?? string.Empty;

    if (string.IsNullOrEmpty(city) || city.Length > 50)
    {
        Console.WriteLine("Email is required (max 100 characters).");
        return;
    }

    using var db = new ShopContext();
    db.Customers.Add(new Customer { Name = name, Email = email, City = city, });
    try
    {
        // Spara våra ändringar
        await db.SaveChangesAsync();
        Console.WriteLine("Product added!");
    }
    catch (DbUpdateException exception)
    {
        Console.WriteLine("DB Error (Maybe duplicate?)! " + exception.GetBaseException().Message);
    }
}

static async Task DeleteCustomerAsync(int id)
{
    using var db = new ShopContext();

    // Hämta raden du vill ta bort igen
    var customer = await db.Customers.FirstOrDefaultAsync((c => c.CustomerId == id));
    if (customer == null)
    {
        Console.WriteLine("Customer not found!");
        return;
    }

    db.Customers.Remove(customer);
    try
    {
        await db.SaveChangesAsync();
        Console.WriteLine("Customer deleted!");
    }
    catch (DbUpdateException exception)
    {
        Console.WriteLine(exception.Message);
    }
}


// List Order Summary View = Makes it so that we can see a summary of orders with customer info
static async Task ListOrderSummary()
{
    using var db = new ShopContext();

    var summaries = await db.OrderSummaries.OrderByDescending(o => o.OrderDate).ToListAsync();

    Console.WriteLine("\n OrderId | OrderDate | TotalAmount | CustomerEmail");
    foreach ( var summary in summaries)
    {
        Console.WriteLine($"{summary.OrderId} | {summary.OrderDate:d} | {summary.TotalAmount} |{summary.CustomerEmail} ");
    }
}

static async Task EditCustomerAsync(int id)
{
    using var db = new ShopContext();

    // Hämta radre vi vill uppdatera
    var customer = await db.Customers.FirstOrDefaultAsync((x => x.CustomerId == id));
    if (customer == null)
    {
        Console.WriteLine("Category not found");
        return;
    }

    // Visar nuvarande värden; Uppdatera namn för en specifik category
    Console.Write($"{customer.Name} ");
    var name = Console.ReadLine()?.Trim() ?? string.Empty;
    if (!string.IsNullOrEmpty(name))
    {
        customer.Name = name;
    }

    // Uppdatera email för en specifik Person
    Console.Write($"{customer.Email} ");
    var email = Console.ReadLine()?.Trim() ?? string.Empty;
    if (!string.IsNullOrEmpty(email))
    {
        customer.Email = email;
    }

    // Uppdatera email för en specifik Person
    Console.Write($"{customer.City} ");
    var city = Console.ReadLine()?.Trim() ?? string.Empty;
    if (!string.IsNullOrEmpty(city))
    {
        customer.City = city;
    }

    // Uppdatera
    try
    {
        await db.SaveChangesAsync();
        Console.WriteLine("Edited!");
    }
    catch (DbUpdateException exception)
    {
        Console.WriteLine(exception.Message);
    }
}

static async Task AddOrderAsync()
{
    using var db = new ShopContext();

    var customers = await db.Customers.AsNoTracking().OrderBy(o => o.CustomerId).ToListAsync();
    if (!customers.Any())
    {
        Console.WriteLine("No Customers found");
        return;
    }

    foreach (var customer in customers)
    {
        Console.WriteLine($"{customer.CustomerId} | {customer.Name} | {customer.Email}");
    }

    Console.Write("CustomerId: ");
    if(!int.TryParse(Console.ReadLine(), out var customerId) ||
        !customers.Any(c => c.CustomerId == customerId))
    {
        Console.WriteLine("Invalid input of customerId");
    }

    var order = new Order
    {
        CustomerId = customerId,
        OrderDate = DateTime.Today,
        Status = "Pending",
        TotalAmount = 0
    };

    var orderRows = new List<OrderRow>();

    while (true)
    {
        Console.Write("Add order row? Y/N");
        var answer = Console.ReadLine()?.Trim().ToLowerInvariant();
        if (answer == "y") break;

        var products = await db.Products.AsNoTracking()
            .OrderBy(o => o.ProductId).ToListAsync();
        if (!products.Any())
        {
            Console.WriteLine("No Products Found");
            return;
        }

        foreach (var product in products)
        {
            Console.WriteLine($"{product.ProductId} | {product.Name} | {product.Price}");
        }

        Console.Write("ProductId: ");
        if (!int.TryParse(Console.ReadLine(), out var productId))
        {
            Console.WriteLine("Invalid input of productId");
            continue;
        }

        var chosenProduct = await db.Products.FirstOrDefaultAsync(p => p.ProductId == productId);
        if(chosenProduct == null)
        {
            Console.WriteLine("Product not found");
            continue;
        }

        Console.Write("Quantity: ");
        if (!int.TryParse(Console.ReadLine(), out var quantity) || quantity <= 0)
        {
            Console.WriteLine("Invalid input of quantity");
            continue;
        }

        var row = new OrderRow
        {
            ProductId = productId,
            Quantity = quantity,
            UnitPrice = chosenProduct.Price,
        };

        orderRows.Add(row);

    }

    order.OrderRows = orderRows;
    order.TotalAmount = orderRows.Sum(o => o.UnitPrice * o.Quantity);

    db.Orders.Add(order);

    try
    {
        await db.SaveChangesAsync();
        Console.WriteLine($"Order {order.OrderId} created!");
    } 
    catch (DbUpdateException exception)
    {
        Console.WriteLine("DB Error: " + exception.GetBaseException().Message);
    }

}

static async Task ListOrdersPagedAsync(int page, int pageSize)
{
    using var db = new ShopContext();

    var query = db.Orders.Include(o => o.Customer).AsNoTracking().OrderByDescending(o => o.OrderDate);

    var totalOrderCount = await query.CountAsync();
    var totalPages = (int)Math.Ceiling(totalOrderCount / (double)pageSize);

    /* total count = 50 ordrar, vi har pageSize av 10 -> 50 / 10 = 5
     * totalCount = 50 Ordrar / 1 Antal ordrar per sida = 50 sidor
     * Ordrar
     * -----
     * 1..10
     * 
     * [0] Sida 1. - [1] Sida 2. - [2] Sida 3.
     * 1..10 - 11...21 - 22 - 32 ..
     * 
     * 
     * 
     */
    var order = await query.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();

    Console.WriteLine($"Page={page}/{totalPages} | pageSize= {pageSize}");
    foreach(var o in order)
    {
        Console.WriteLine($"OrderId: {o.OrderId} | Date: {o.OrderDate:d} | TotalAmount: ${o.TotalAmount} ");
    }
}

static async Task OrderMenuAsync()
{
    while (true)
    {

        Console.WriteLine("\n 1 = ListOrder | 2= AddOrder | 3 = EditOrder | 4 = NumberOfOrders | 5 = ListOrders | 6 = DeleteOrder | 7 = Menu");
        string orderMenu = Console.ReadLine()?.Trim() ?? string.Empty;
        switch (orderMenu)
        {
            case "1":
                await ListOrdersPagedAsync(1, 10);
                break;
            case "2":
                await AddOrderAsync();
                break;
            case "3":
                await EditOrderAsync();
                break;
            case "4":
                await NumberOfOrdersAsync();
                break;
            case "5":
                await ListOrdersAsync();
                break;
            case "6":
                await DeleteOrderAsync();
                break;
            case "7":
                await Menu();
                break;
            default:
                Console.WriteLine("Please select a valid option");
                break;

        }
    }
}

static async Task ListOrdersAsync()
{
    using var db = new ShopContext();

    var orders = await db.Orders.AsNoTracking()
        .Include(o => o.Customer)
        .Include(o => o.OrderRows)
        .OrderByDescending(o => o.OrderRows)
        .ToListAsync();

    foreach (var order in orders)
    {
        Console.WriteLine($"{order.OrderId} | {order.Customer?.Email} | {order.OrderRows}");
    }
}

static async Task EditOrderAsync()
{

}

static async Task DeleteOrderAsync()
{

}

static async Task ProductMenuAsync()
{
    while (true)
    {

        Console.WriteLine("\n 1 = ListProduct | 2 = AddProduct | 3 = EditProduct | 4 = DeleteProduct | 6 = ProductSales | 5 = return");
        string productMenu = Console.ReadLine()?.ToLower() ?? string.Empty;
        switch (productMenu)
        {
            case "1":
                //await ListProductAsync();
                break;
            case "2":
                //await AddProductAsync();
                break;
            case "3":
                //await EditProductAsync();
                break;
            case "4":
                //await DeleteProductAsync();
                break;
            case "5":
                await Menu();
                break;
            case "6":
                await ProductSalesAsync();
                break;
            default:
                Console.WriteLine("Please select a valid option");
                break;

        }
    }
}

static async Task ProductSalesAsync()
{
    using var db = new ShopContext();
    var products = await db.ProductSalesViews
        .OrderByDescending(p => p.ProductId)
        .ToListAsync();
    Console.WriteLine("\n ProductId | ProductName | TotalSales");
    foreach (var product in products)
    {
        Console.WriteLine($"{product.ProductId} | {product.Name} | {product.TotalQuantitySold}");
    }
}

