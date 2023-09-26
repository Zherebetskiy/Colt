using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Colt.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class update_order_product_delete_behavior : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OrderProduct_CustomerProducts_CustomerProductId",
                table: "OrderProduct");

            migrationBuilder.DropForeignKey(
                name: "FK_OrderProduct_Orders_OrderId",
                table: "OrderProduct");

            migrationBuilder.AlterColumn<int>(
                name: "CustomerProductId",
                table: "OrderProduct",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_OrderProduct_CustomerProducts_CustomerProductId",
                table: "OrderProduct",
                column: "CustomerProductId",
                principalTable: "CustomerProducts",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_OrderProduct_Orders_OrderId",
                table: "OrderProduct",
                column: "OrderId",
                principalTable: "Orders",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OrderProduct_CustomerProducts_CustomerProductId",
                table: "OrderProduct");

            migrationBuilder.DropForeignKey(
                name: "FK_OrderProduct_Orders_OrderId",
                table: "OrderProduct");

            migrationBuilder.AlterColumn<int>(
                name: "CustomerProductId",
                table: "OrderProduct",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_OrderProduct_CustomerProducts_CustomerProductId",
                table: "OrderProduct",
                column: "CustomerProductId",
                principalTable: "CustomerProducts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_OrderProduct_Orders_OrderId",
                table: "OrderProduct",
                column: "OrderId",
                principalTable: "Orders",
                principalColumn: "Id");
        }
    }
}
