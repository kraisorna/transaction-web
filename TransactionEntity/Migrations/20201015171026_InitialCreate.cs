using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace TransactionEntity.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TransactionFile",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Content = table.Column<byte[]>(nullable: true),
                    FileName = table.Column<string>(nullable: true),
                    Note = table.Column<string>(nullable: true),
                    Size = table.Column<long>(nullable: false),
                    UploadDate = table.Column<DateTimeOffset>(nullable: false),
                    FileStatus = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TransactionFile", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TransactionImport",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TransactionId = table.Column<string>(maxLength: 50, nullable: true),
                    Amount = table.Column<decimal>(type: "decimal(18, 2)", nullable: true),
                    CurrencyCode = table.Column<string>(fixedLength: true, maxLength: 3, nullable: true),
                    Status = table.Column<string>(fixedLength: true, maxLength: 1, nullable: true),
                    TransactionDate = table.Column<DateTimeOffset>(nullable: true),
                    ImportStatus = table.Column<int>(nullable: false),
                    Note = table.Column<string>(nullable: true),
                    ImportDate = table.Column<DateTimeOffset>(nullable: false),
                    TransactionFileId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TransactionImport", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TransactionImport_TransactionFile_TransactionFileId",
                        column: x => x.TransactionFileId,
                        principalTable: "TransactionFile",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Transaction",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TransactionIdentificator = table.Column<string>(maxLength: 50, nullable: false),
                    Amount = table.Column<decimal>(type: "decimal(18, 2)", nullable: false),
                    CurrencyCode = table.Column<string>(fixedLength: true, maxLength: 3, nullable: false),
                    Status = table.Column<string>(fixedLength: true, maxLength: 1, nullable: false),
                    TransactionDate = table.Column<DateTimeOffset>(nullable: false),
                    TransactionImportId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Transaction", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Transaction_TransactionImport_TransactionImportId",
                        column: x => x.TransactionImportId,
                        principalTable: "TransactionImport",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Transaction_TransactionImportId",
                table: "Transaction",
                column: "TransactionImportId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_TransactionImport_TransactionFileId",
                table: "TransactionImport",
                column: "TransactionFileId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Transaction");

            migrationBuilder.DropTable(
                name: "TransactionImport");

            migrationBuilder.DropTable(
                name: "TransactionFile");
        }
    }
}
