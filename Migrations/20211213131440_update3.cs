using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Restaurant_system_new.Migrations
{
    public partial class update3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RowVersion",
                table: "Dish");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<byte[]>(
                name: "RowVersion",
                table: "Dish",
                type: "rowversion",
                rowVersion: true,
                nullable: true);
        }
    }
}
