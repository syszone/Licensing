using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace License.Manager.Data.Migrations
{
    public partial class InitialDB : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Customer",
                columns: table => new
                {
                    CustomerId = table.Column<Guid>(nullable: false, defaultValueSql: "newid()"),
                    Name = table.Column<string>(nullable: true),
                    Company = table.Column<string>(nullable: true),
                    Email = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Customer", x => x.CustomerId);
                });

            migrationBuilder.CreateTable(
                name: "KeyPair",
                columns: table => new
                {
                    KeyPairId = table.Column<Guid>(nullable: false),
                    EncryptedPrivateKey = table.Column<string>(nullable: true),
                    PublicKey = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_KeyPair", x => x.KeyPairId);
                });

            migrationBuilder.CreateTable(
                name: "Product",
                columns: table => new
                {
                    ProductId = table.Column<Guid>(nullable: false, defaultValueSql: "newid()"),
                    Name = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    ExpiryTerm = table.Column<int>(nullable: false),
                    KeyPairId = table.Column<Guid>(nullable: true),
                    LicenseType = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Product", x => x.ProductId);
                    table.ForeignKey(
                        name: "FK_Product_KeyPair_KeyPairId",
                        column: x => x.KeyPairId,
                        principalTable: "KeyPair",
                        principalColumn: "KeyPairId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ProductFeature",
                columns: table => new
                {
                    FeatureId = table.Column<Guid>(nullable: false, defaultValueSql: "newid()"),
                    ProductId = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    Value = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductFeature", x => x.FeatureId);
                    table.ForeignKey(
                        name: "FK_ProductFeature_Product_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Product",
                        principalColumn: "ProductId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Product_KeyPairId",
                table: "Product",
                column: "KeyPairId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductFeature_ProductId",
                table: "ProductFeature",
                column: "ProductId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Customer");

            migrationBuilder.DropTable(
                name: "ProductFeature");

            migrationBuilder.DropTable(
                name: "Product");

            migrationBuilder.DropTable(
                name: "KeyPair");
        }
    }
}
