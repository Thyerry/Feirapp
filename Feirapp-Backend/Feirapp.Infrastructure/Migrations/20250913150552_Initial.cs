using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Feirapp.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "cests",
                columns: table => new
                {
                    code = table.Column<string>(type: "text", nullable: false),
                    segment = table.Column<string>(type: "text", nullable: true),
                    description = table.Column<string>(type: "text", nullable: true),
                    ncm_code = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_cests", x => x.code);
                });

            migrationBuilder.CreateTable(
                name: "grocery_items",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    name = table.Column<string>(type: "character varying(1024)", maxLength: 1024, nullable: false),
                    brand = table.Column<string>(type: "character varying(1024)", maxLength: 1024, nullable: true),
                    description = table.Column<string>(type: "text", nullable: true),
                    image_url = table.Column<string>(type: "character varying(1024)", maxLength: 1024, nullable: true),
                    barcode = table.Column<string>(type: "text", nullable: false),
                    measure_unit = table.Column<string>(type: "character varying(25)", maxLength: 25, nullable: false),
                    ncm_code = table.Column<string>(type: "text", nullable: true),
                    cest_code = table.Column<string>(type: "text", nullable: true),
                    alt_names = table.Column<List<string>>(type: "text[]", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_grocery_items", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "invoices",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    code = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false),
                    url = table.Column<string>(type: "character varying(1024)", maxLength: 1024, nullable: false),
                    scan_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    user_id = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_invoices", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "ncms",
                columns: table => new
                {
                    id = table.Column<string>(type: "text", nullable: false),
                    description = table.Column<string>(type: "text", nullable: true),
                    last_update = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ncms", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "price_logs",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    price = table.Column<decimal>(type: "numeric(10,2)", precision: 10, scale: 2, nullable: false),
                    log_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    grocery_item_id = table.Column<Guid>(type: "uuid", nullable: false),
                    barcode = table.Column<string>(type: "text", nullable: false),
                    product_code = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    invoice_id = table.Column<Guid>(type: "uuid", nullable: true),
                    store_id = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_price_logs", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "stores",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    name = table.Column<string>(type: "character varying(1024)", maxLength: 1024, nullable: false),
                    alt_names = table.Column<List<string>>(type: "text[]", nullable: true),
                    cnpj = table.Column<string>(type: "text", nullable: true),
                    cep = table.Column<string>(type: "character varying(8)", maxLength: 8, nullable: true),
                    street = table.Column<string>(type: "character varying(1024)", maxLength: 1024, nullable: true),
                    street_number = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    neighborhood = table.Column<string>(type: "character varying(1024)", maxLength: 1024, nullable: true),
                    city_name = table.Column<string>(type: "character varying(1024)", maxLength: 1024, nullable: true),
                    state = table.Column<string>(type: "character varying(2)", maxLength: 2, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_stores", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "users",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    name = table.Column<string>(type: "character varying(1024)", maxLength: 1024, nullable: false),
                    email = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false),
                    password = table.Column<string>(type: "character(50)", fixedLength: true, maxLength: 50, nullable: false),
                    password_salt = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    last_login = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    failed_login_attempts = table.Column<int>(type: "integer", nullable: false),
                    status = table.Column<string>(name: "status'", type: "character varying(20)", maxLength: 20, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_users", x => x.id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_grocery_items_barcode",
                table: "grocery_items",
                column: "barcode");

            migrationBuilder.CreateIndex(
                name: "IX_grocery_items_name",
                table: "grocery_items",
                column: "name");

            migrationBuilder.CreateIndex(
                name: "IX_invoices_code",
                table: "invoices",
                column: "code");

            migrationBuilder.CreateIndex(
                name: "IX_invoices_scan_date",
                table: "invoices",
                column: "scan_date");

            migrationBuilder.CreateIndex(
                name: "idx_pricelogs_logdate_desc",
                table: "price_logs",
                column: "log_date");

            migrationBuilder.CreateIndex(
                name: "IX_price_logs_barcode",
                table: "price_logs",
                column: "barcode");

            migrationBuilder.CreateIndex(
                name: "IX_price_logs_grocery_item_id",
                table: "price_logs",
                column: "grocery_item_id");

            migrationBuilder.CreateIndex(
                name: "IX_price_logs_grocery_item_id_barcode_log_date_store_id",
                table: "price_logs",
                columns: new[] { "grocery_item_id", "barcode", "log_date", "store_id" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_price_logs_invoice_id",
                table: "price_logs",
                column: "invoice_id");

            migrationBuilder.CreateIndex(
                name: "IX_price_logs_store_id",
                table: "price_logs",
                column: "store_id");

            migrationBuilder.CreateIndex(
                name: "IX_stores_alt_names",
                table: "stores",
                column: "alt_names")
                .Annotation("Npgsql:IndexMethod", "GIN");

            migrationBuilder.CreateIndex(
                name: "IX_stores_city_name",
                table: "stores",
                column: "city_name");

            migrationBuilder.CreateIndex(
                name: "IX_stores_cnpj",
                table: "stores",
                column: "cnpj",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_stores_name",
                table: "stores",
                column: "name");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "cests");

            migrationBuilder.DropTable(
                name: "grocery_items");

            migrationBuilder.DropTable(
                name: "invoices");

            migrationBuilder.DropTable(
                name: "ncms");

            migrationBuilder.DropTable(
                name: "price_logs");

            migrationBuilder.DropTable(
                name: "stores");

            migrationBuilder.DropTable(
                name: "users");
        }
    }
}
