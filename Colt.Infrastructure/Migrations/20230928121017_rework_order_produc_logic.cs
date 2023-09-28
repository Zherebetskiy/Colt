using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Colt.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class rework_order_produc_logic : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OrderProduct_CustomerProducts_CustomerProductId",
                table: "OrderProduct");

            migrationBuilder.DropIndex(
                name: "IX_OrderProduct_CustomerProductId",
                table: "OrderProduct");

            migrationBuilder.DropColumn(
                name: "CustomerProductId",
                table: "OrderProduct");

            migrationBuilder.AddColumn<string>(
                name: "ProductName",
                table: "OrderProduct",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<decimal>(
                name: "ProductPrice",
                table: "OrderProduct",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ProductName",
                table: "OrderProduct");

            migrationBuilder.DropColumn(
                name: "ProductPrice",
                table: "OrderProduct");

            migrationBuilder.AddColumn<int>(
                name: "CustomerProductId",
                table: "OrderProduct",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_OrderProduct_CustomerProductId",
                table: "OrderProduct",
                column: "CustomerProductId");

            migrationBuilder.AddForeignKey(
                name: "FK_OrderProduct_CustomerProducts_CustomerProductId",
                table: "OrderProduct",
                column: "CustomerProductId",
                principalTable: "CustomerProducts",
                principalColumn: "Id");
        }
    }
}
