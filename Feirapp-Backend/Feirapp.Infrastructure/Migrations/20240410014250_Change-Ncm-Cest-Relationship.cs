using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Feirapp.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class ChangeNcmCestRelationship : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Cests_Ncms_NcmCode",
                table: "Cests");

            migrationBuilder.DropForeignKey(
                name: "FK_GroceryItems_Cests_CestCode",
                table: "GroceryItems");

            migrationBuilder.DropForeignKey(
                name: "FK_GroceryItems_Ncms_NcmCode",
                table: "GroceryItems");

            migrationBuilder.AlterColumn<string>(
                name: "NcmCode",
                table: "GroceryItems",
                type: "varchar(255)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(255)")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "CestCode",
                table: "GroceryItems",
                type: "varchar(255)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(255)")
                .Annotation("MySql:CharSet", "utf8mb4")
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

            migrationBuilder.AlterColumn<string>(
                name: "NcmCode",
                table: "Cests",
                type: "varchar(255)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(255)")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddForeignKey(
                name: "FK_Cests_Ncms_NcmCode",
                table: "Cests",
                column: "NcmCode",
                principalTable: "Ncms",
                principalColumn: "Code");

            migrationBuilder.AddForeignKey(
                name: "FK_GroceryItems_Cests_CestCode",
                table: "GroceryItems",
                column: "CestCode",
                principalTable: "Cests",
                principalColumn: "Code");

            migrationBuilder.AddForeignKey(
                name: "FK_GroceryItems_Ncms_NcmCode",
                table: "GroceryItems",
                column: "NcmCode",
                principalTable: "Ncms",
                principalColumn: "Code");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Cests_Ncms_NcmCode",
                table: "Cests");

            migrationBuilder.DropForeignKey(
                name: "FK_GroceryItems_Cests_CestCode",
                table: "GroceryItems");

            migrationBuilder.DropForeignKey(
                name: "FK_GroceryItems_Ncms_NcmCode",
                table: "GroceryItems");

            migrationBuilder.UpdateData(
                table: "GroceryItems",
                keyColumn: "NcmCode",
                keyValue: null,
                column: "NcmCode",
                value: "");

            migrationBuilder.AlterColumn<string>(
                name: "NcmCode",
                table: "GroceryItems",
                type: "varchar(255)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(255)",
                oldNullable: true)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.UpdateData(
                table: "GroceryItems",
                keyColumn: "CestCode",
                keyValue: null,
                column: "CestCode",
                value: "");

            migrationBuilder.AlterColumn<string>(
                name: "CestCode",
                table: "GroceryItems",
                type: "varchar(255)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(255)",
                oldNullable: true)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

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

            migrationBuilder.UpdateData(
                table: "Cests",
                keyColumn: "NcmCode",
                keyValue: null,
                column: "NcmCode",
                value: "");

            migrationBuilder.AlterColumn<string>(
                name: "NcmCode",
                table: "Cests",
                type: "varchar(255)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(255)",
                oldNullable: true)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddForeignKey(
                name: "FK_Cests_Ncms_NcmCode",
                table: "Cests",
                column: "NcmCode",
                principalTable: "Ncms",
                principalColumn: "Code",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_GroceryItems_Cests_CestCode",
                table: "GroceryItems",
                column: "CestCode",
                principalTable: "Cests",
                principalColumn: "Code",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_GroceryItems_Ncms_NcmCode",
                table: "GroceryItems",
                column: "NcmCode",
                principalTable: "Ncms",
                principalColumn: "Code",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
