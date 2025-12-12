using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Hemmuppgiftcrud.Migrations
{
    /// <inheritdoc />
    public partial class LastMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CustomerNameHash",
                table: "Customers",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CustomerNameSalt",
                table: "Customers",
                type: "TEXT",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CustomerNameHash",
                table: "Customers");

            migrationBuilder.DropColumn(
                name: "CustomerNameSalt",
                table: "Customers");
        }
    }
}
