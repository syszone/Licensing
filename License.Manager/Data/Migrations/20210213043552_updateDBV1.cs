using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace License.Manager.Data.Migrations
{
    public partial class updateDBV1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Product_KeyPair_KeyPairId",
                table: "Product");

            migrationBuilder.DropIndex(
                name: "IX_Product_KeyPairId",
                table: "Product");

            migrationBuilder.DropColumn(
                name: "ExpiryTerm",
                table: "Product");

            migrationBuilder.DropColumn(
                name: "KeyPairId",
                table: "Product");

            migrationBuilder.DropColumn(
                name: "LicenseType",
                table: "Product");

            migrationBuilder.CreateTable(
                name: "LicenseModel",
                columns: table => new
                {
                    LicenseModelId = table.Column<Guid>(nullable: false),
                    ProductId = table.Column<Guid>(nullable: false, defaultValueSql: "newid()"),
                    Name = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    ExpiryTerm = table.Column<int>(nullable: false),
                    KeyPairId = table.Column<Guid>(nullable: true),
                    LicenseType = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LicenseModel", x => x.LicenseModelId);
                    table.ForeignKey(
                        name: "FK_LicenseModel_KeyPair_KeyPairId",
                        column: x => x.KeyPairId,
                        principalTable: "KeyPair",
                        principalColumn: "KeyPairId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_LicenseModel_Product_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Product",
                        principalColumn: "ProductId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "LicenseModelFeature",
                columns: table => new
                {
                    LicenseModelFeatureId = table.Column<Guid>(nullable: false),
                    LicenseModelId = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    Value = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LicenseModelFeature", x => x.LicenseModelFeatureId);
                    table.ForeignKey(
                        name: "FK_LicenseModelFeature_LicenseModel_LicenseModelId",
                        column: x => x.LicenseModelId,
                        principalTable: "LicenseModel",
                        principalColumn: "LicenseModelId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_LicenseModel_KeyPairId",
                table: "LicenseModel",
                column: "KeyPairId");

            migrationBuilder.CreateIndex(
                name: "IX_LicenseModel_ProductId",
                table: "LicenseModel",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_LicenseModelFeature_LicenseModelId",
                table: "LicenseModelFeature",
                column: "LicenseModelId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "LicenseModelFeature");

            migrationBuilder.DropTable(
                name: "LicenseModel");

            migrationBuilder.AddColumn<int>(
                name: "ExpiryTerm",
                table: "Product",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<Guid>(
                name: "KeyPairId",
                table: "Product",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "LicenseType",
                table: "Product",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Product_KeyPairId",
                table: "Product",
                column: "KeyPairId");

            migrationBuilder.AddForeignKey(
                name: "FK_Product_KeyPair_KeyPairId",
                table: "Product",
                column: "KeyPairId",
                principalTable: "KeyPair",
                principalColumn: "KeyPairId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
