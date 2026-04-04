using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Feirapp.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class changeusertable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "status'",
                table: "users",
                newName: "status");

            migrationBuilder.AlterColumn<string>(
                name: "password",
                table: "users",
                type: "character varying(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character(50)",
                oldFixedLength: true,
                oldMaxLength: 50);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "status",
                table: "users",
                newName: "status'");

            migrationBuilder.AlterColumn<string>(
                name: "password",
                table: "users",
                type: "character(50)",
                fixedLength: true,
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(50)",
                oldMaxLength: 50);
        }
    }
}
