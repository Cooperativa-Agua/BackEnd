using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Bombas",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Nombre = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Descripcion = table.Column<string>(type: "varchar(500)", maxLength: 500, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    EstaEncendida = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    RelayActivo = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    SalvaMotorActivo = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    FlujometroActivo = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    FechaCreacion = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    UltimaActualizacion = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Bombas", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Email = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Password = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.InsertData(
                table: "Bombas",
                columns: new[] { "Id", "Descripcion", "EstaEncendida", "FechaCreacion", "FlujometroActivo", "Nombre", "RelayActivo", "SalvaMotorActivo", "UltimaActualizacion" },
                values: new object[,]
                {
                    { 1, "Bomba principal del sector A", false, new DateTime(2025, 7, 29, 19, 52, 51, 48, DateTimeKind.Utc).AddTicks(4684), false, "Bomba 1", false, false, new DateTime(2025, 7, 29, 19, 52, 51, 48, DateTimeKind.Utc).AddTicks(4685) },
                    { 2, "Bomba secundaria del sector A", false, new DateTime(2025, 7, 29, 19, 52, 51, 48, DateTimeKind.Utc).AddTicks(4687), false, "Bomba 2", false, false, new DateTime(2025, 7, 29, 19, 52, 51, 48, DateTimeKind.Utc).AddTicks(4687) },
                    { 3, "Bomba de respaldo del sector B", false, new DateTime(2025, 7, 29, 19, 52, 51, 48, DateTimeKind.Utc).AddTicks(4689), false, "Bomba 3", false, false, new DateTime(2025, 7, 29, 19, 52, 51, 48, DateTimeKind.Utc).AddTicks(4689) }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Bombas");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
