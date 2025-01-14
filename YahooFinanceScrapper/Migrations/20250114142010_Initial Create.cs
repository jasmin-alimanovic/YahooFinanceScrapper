using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace YahooFinanceScrapper.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Tickers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TickerName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CompanyName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MarketCap = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    YearFounded = table.Column<int>(type: "int", nullable: true),
                    NumberOfEmployees = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    HeadquartersCity = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    HeadquartersState = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ClosedPriceDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    PreviousClosePrice = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    OpenPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tickers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TickerSymbols",
                columns: table => new
                {
                    Symbol = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TickerSymbols", x => x.Symbol);
                });

            migrationBuilder.InsertData(
                table: "TickerSymbols",
                columns: new[] { "Symbol", "Name" },
                values: new object[,]
                {
                    { "AAPL", "Apple Inc." },
                    { "AMZN", "Amazon.com, Inc." },
                    { "AVGO", "Broadcom Inc." },
                    { "CEG", "Constellation Energy Corporatio" },
                    { "CVS", "CVS Health Corporation" },
                    { "GCTK", "GlucoTrack, Inc." },
                    { "INTC", "Intel Corporation" },
                    { "META", "Meta Platforms, Inc." },
                    { "MSFT", "Microsoft Corporation" },
                    { "NVDA", "NVIDIA Corporation" },
                    { "PLTR", "Palantir Technologies Inc." },
                    { "QQQ", "Invesco QQQ Trust, Series 1" },
                    { "SHOP", "Shopify Inc." },
                    { "TSLA", "Tesla, Inc." },
                    { "WBA", "Walgreens Boots Alliance, Inc." }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Tickers");

            migrationBuilder.DropTable(
                name: "TickerSymbols");
        }
    }
}
