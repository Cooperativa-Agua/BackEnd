using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Tanques : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Tanques",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Nombre = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Descripcion = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    NivelAgua = table.Column<double>(type: "double", nullable: false),
                    CapacidadMaxima = table.Column<double>(type: "double", nullable: false),
                    EstaActivo = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    FechaCreacion = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    UltimaActualizacion = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tanques", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.UpdateData(
                table: "Bombas",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "FechaCreacion", "UltimaActualizacion" },
                values: new object[] { new DateTime(2025, 8, 12, 18, 41, 59, 694, DateTimeKind.Utc).AddTicks(9231), new DateTime(2025, 8, 12, 18, 41, 59, 694, DateTimeKind.Utc).AddTicks(9232) });

            migrationBuilder.UpdateData(
                table: "Bombas",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "FechaCreacion", "UltimaActualizacion" },
                values: new object[] { new DateTime(2025, 8, 12, 18, 41, 59, 694, DateTimeKind.Utc).AddTicks(9234), new DateTime(2025, 8, 12, 18, 41, 59, 694, DateTimeKind.Utc).AddTicks(9235) });

            migrationBuilder.UpdateData(
                table: "Bombas",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "FechaCreacion", "UltimaActualizacion" },
                values: new object[] { new DateTime(2025, 8, 12, 18, 41, 59, 694, DateTimeKind.Utc).AddTicks(9237), new DateTime(2025, 8, 12, 18, 41, 59, 694, DateTimeKind.Utc).AddTicks(9237) });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Tanques");

            migrationBuilder.UpdateData(
                table: "Bombas",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "FechaCreacion", "UltimaActualizacion" },
                values: new object[] { new DateTime(2025, 7, 29, 19, 52, 51, 48, DateTimeKind.Utc).AddTicks(4684), new DateTime(2025, 7, 29, 19, 52, 51, 48, DateTimeKind.Utc).AddTicks(4685) });

            migrationBuilder.UpdateData(
                table: "Bombas",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "FechaCreacion", "UltimaActualizacion" },
                values: new object[] { new DateTime(2025, 7, 29, 19, 52, 51, 48, DateTimeKind.Utc).AddTicks(4687), new DateTime(2025, 7, 29, 19, 52, 51, 48, DateTimeKind.Utc).AddTicks(4687) });

            migrationBuilder.UpdateData(
                table: "Bombas",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "FechaCreacion", "UltimaActualizacion" },
                values: new object[] { new DateTime(2025, 7, 29, 19, 52, 51, 48, DateTimeKind.Utc).AddTicks(4689), new DateTime(2025, 7, 29, 19, 52, 51, 48, DateTimeKind.Utc).AddTicks(4689) });
        }
    }
}
