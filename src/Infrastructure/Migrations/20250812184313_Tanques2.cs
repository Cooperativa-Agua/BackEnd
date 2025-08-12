using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Tanques2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Nombre",
                table: "Tanques",
                type: "varchar(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "longtext")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "Descripcion",
                table: "Tanques",
                type: "varchar(500)",
                maxLength: 500,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "longtext")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.UpdateData(
                table: "Bombas",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "FechaCreacion", "UltimaActualizacion" },
                values: new object[] { new DateTime(2025, 8, 12, 18, 43, 12, 939, DateTimeKind.Utc).AddTicks(4762), new DateTime(2025, 8, 12, 18, 43, 12, 939, DateTimeKind.Utc).AddTicks(4762) });

            migrationBuilder.UpdateData(
                table: "Bombas",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "FechaCreacion", "UltimaActualizacion" },
                values: new object[] { new DateTime(2025, 8, 12, 18, 43, 12, 939, DateTimeKind.Utc).AddTicks(4765), new DateTime(2025, 8, 12, 18, 43, 12, 939, DateTimeKind.Utc).AddTicks(4765) });

            migrationBuilder.UpdateData(
                table: "Bombas",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "FechaCreacion", "UltimaActualizacion" },
                values: new object[] { new DateTime(2025, 8, 12, 18, 43, 12, 939, DateTimeKind.Utc).AddTicks(4767), new DateTime(2025, 8, 12, 18, 43, 12, 939, DateTimeKind.Utc).AddTicks(4768) });

            migrationBuilder.InsertData(
                table: "Tanques",
                columns: new[] { "Id", "CapacidadMaxima", "Descripcion", "EstaActivo", "FechaCreacion", "NivelAgua", "Nombre", "UltimaActualizacion" },
                values: new object[,]
                {
                    { 1, 10000.0, "Tanque principal de almacenamiento de agua", true, new DateTime(2025, 8, 12, 18, 43, 12, 939, DateTimeKind.Utc).AddTicks(4871), 75.0, "Tanque Principal", new DateTime(2025, 8, 12, 18, 43, 12, 939, DateTimeKind.Utc).AddTicks(4872) },
                    { 2, 5000.0, "Tanque de reserva para el sector A", true, new DateTime(2025, 8, 12, 18, 43, 12, 939, DateTimeKind.Utc).AddTicks(4873), 45.0, "Tanque Secundario", new DateTime(2025, 8, 12, 18, 43, 12, 939, DateTimeKind.Utc).AddTicks(4874) },
                    { 3, 3000.0, "Tanque elevado para distribución por gravedad", true, new DateTime(2025, 8, 12, 18, 43, 12, 939, DateTimeKind.Utc).AddTicks(4876), 15.0, "Tanque Elevado", new DateTime(2025, 8, 12, 18, 43, 12, 939, DateTimeKind.Utc).AddTicks(4876) }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Tanques",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Tanques",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Tanques",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.AlterColumn<string>(
                name: "Nombre",
                table: "Tanques",
                type: "longtext",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(100)",
                oldMaxLength: 100)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "Descripcion",
                table: "Tanques",
                type: "longtext",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(500)",
                oldMaxLength: 500)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

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
    }
}
