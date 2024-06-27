using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Feirapp.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class groceryitembarcodenotrequired : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_GroceryItems_Barcode",
                table: "GroceryItems");

            migrationBuilder.AddColumn<DateTime>(
                name: "LastUpdate",
                table: "Ncms",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AlterColumn<string>(
                name: "MeasureUnit",
                table: "GroceryItems",
                type: "longtext",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int")
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.UpdateData(
                table: "GroceryItems",
                keyColumn: "Barcode",
                keyValue: null,
                column: "Barcode",
                value: "");

            migrationBuilder.AlterColumn<string>(
                name: "Barcode",
                table: "GroceryItems",
                type: "varchar(255)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(255)",
                oldNullable: true)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_GroceryItems_Barcode",
                table: "GroceryItems",
                column: "Barcode");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_GroceryItems_Barcode",
                table: "GroceryItems");

            migrationBuilder.DropColumn(
                name: "LastUpdate",
                table: "Ncms");

            migrationBuilder.AlterColumn<int>(
                name: "MeasureUnit",
                table: "GroceryItems",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "longtext")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "Barcode",
                table: "GroceryItems",
                type: "varchar(255)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(255)")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_GroceryItems_Barcode",
                table: "GroceryItems",
                column: "Barcode",
                unique: true);
        }
    }
}
