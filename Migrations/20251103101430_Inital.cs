using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LEARN_MVVM.Migrations
{
    /// <inheritdoc />
    public partial class Inital : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Temperatures",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    TimeStamp = table.Column<DateTimeOffset>(type: "TEXT", nullable: false),
                    City = table.Column<string>(type: "TEXT", maxLength: 200, nullable: false),
                    Temp = table.Column<double>(type: "REAL", precision: 5, scale: 2, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Temperatures", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Temperatures_City",
                table: "Temperatures",
                column: "City",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Temperatures");
        }
    }
}
