using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Colt.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateOrderProducEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ActualItemsAmount",
                table: "OrderProduct");

            migrationBuilder.DropColumn(
                name: "OrderdItemsAmount",
                table: "OrderProduct");

            migrationBuilder.RenameColumn(
                name: "ProductPrice",
                table: "OrderProduct",
                newName: "TotalPrice");

            migrationBuilder.RenameColumn(
                name: "OrderdItemsWeight",
                table: "OrderProduct",
                newName: "OrderedWeight");

            migrationBuilder.RenameColumn(
                name: "ActualItemsWeight",
                table: "OrderProduct",
                newName: "ActualWeight");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "TotalPrice",
                table: "OrderProduct",
                newName: "ProductPrice");

            migrationBuilder.RenameColumn(
                name: "OrderedWeight",
                table: "OrderProduct",
                newName: "OrderdItemsWeight");

            migrationBuilder.RenameColumn(
                name: "ActualWeight",
                table: "OrderProduct",
                newName: "ActualItemsWeight");

            migrationBuilder.AddColumn<int>(
                name: "ActualItemsAmount",
                table: "OrderProduct",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "OrderdItemsAmount",
                table: "OrderProduct",
                type: "int",
                nullable: true);
        }
    }
}
