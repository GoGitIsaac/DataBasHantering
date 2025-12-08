using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Hemmuppgiftcrud.Migrations
{
    /// <inheritdoc />
    public partial class AddView2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
            
            CREATE VIEW IF NOT EXISTS CustomerOrderCountView AS
            SELECT 
                c.CustomerId,
                c.Name,
                c.Email,
                COUNT(o.OrderId) AS NumberOfOrders
            FROM Customers c 
            LEFT JOIN Orders o ON o.CustomerId = c.CustomerId
            GROUP BY c.CustomerId, c.Name, c.Email
"); // LEFT JOIN = Includes Customers that don't order. RIGHT JOIN = Would Exclude Customers without orders
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@" 
            
            DROP VIEW IF EXISTS CustomerOrderCountView
");
        }
    }
}
