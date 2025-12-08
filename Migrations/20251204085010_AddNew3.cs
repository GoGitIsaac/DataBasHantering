using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Hemmuppgiftcrud.Migrations
{
    // ProductID - p.ProductId
    // Product - Name p.Name
    // TotalQuantitySold - OrderRow tabellen

    /// <inheritdoc />
    public partial class AddNew3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@" 

            CREATE VIEW IF NOT EXISTS ProductSalesView AS
            SELECT 
                p.ProductId,
                p.Name,
                IFNULL(SUM(orw.Quantity), 0) AS TotalQuantitySold
            FROM Products p
            LEFT JOIN OrderRows orw ON orw.ProductId = p.ProductId
            GROUP BY p.ProductId, p.Name;
            ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@" 
            
            DROP VIEW IF EXISTS ProductSalesView
");
        }
    }
}
