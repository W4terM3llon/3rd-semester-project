using Microsoft.EntityFrameworkCore.Migrations;

namespace Restaurant_system_new.Migrations
{
    public partial class orderclear : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Order_Discount_DiscountDbId",
                table: "Order");

            migrationBuilder.DropForeignKey(
                name: "FK_Order_OrderStage_OrderStageDbId",
                table: "Order");

            migrationBuilder.DropForeignKey(
                name: "FK_Order_Payment_PaymentDbId",
                table: "Order");

            migrationBuilder.DropIndex(
                name: "IX_Order_DiscountDbId",
                table: "Order");

            migrationBuilder.DropIndex(
                name: "IX_Order_OrderStageDbId",
                table: "Order");

            migrationBuilder.DropIndex(
                name: "IX_Order_PaymentDbId",
                table: "Order");

            migrationBuilder.DropColumn(
                name: "DiscountDbId",
                table: "Order");

            migrationBuilder.DropColumn(
                name: "OrderStageDbId",
                table: "Order");

            migrationBuilder.DropColumn(
                name: "PaymentDbId",
                table: "Order");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "DiscountDbId",
                table: "Order",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "OrderStageDbId",
                table: "Order",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "PaymentDbId",
                table: "Order",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Order_DiscountDbId",
                table: "Order",
                column: "DiscountDbId");

            migrationBuilder.CreateIndex(
                name: "IX_Order_OrderStageDbId",
                table: "Order",
                column: "OrderStageDbId");

            migrationBuilder.CreateIndex(
                name: "IX_Order_PaymentDbId",
                table: "Order",
                column: "PaymentDbId");

            migrationBuilder.AddForeignKey(
                name: "FK_Order_Discount_DiscountDbId",
                table: "Order",
                column: "DiscountDbId",
                principalTable: "Discount",
                principalColumn: "DbId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Order_OrderStage_OrderStageDbId",
                table: "Order",
                column: "OrderStageDbId",
                principalTable: "OrderStage",
                principalColumn: "DbId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Order_Payment_PaymentDbId",
                table: "Order",
                column: "PaymentDbId",
                principalTable: "Payment",
                principalColumn: "DbId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
