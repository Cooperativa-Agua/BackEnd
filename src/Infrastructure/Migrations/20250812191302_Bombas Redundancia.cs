using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class BombasRedundancia : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "EsBombaReserva",
                table: "Bombas",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "Estado",
                table: "Bombas",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "HorasOperacion",
                table: "Bombas",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Prioridad",
                table: "Bombas",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "TipoFalla",
                table: "Bombas",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "UltimaFalla",
                table: "Bombas",
                type: "datetime(6)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "UltimoMantenimiento",
                table: "Bombas",
                type: "datetime(6)",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "Bombas",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "EsBombaReserva", "Estado", "FechaCreacion", "HorasOperacion", "Prioridad", "TipoFalla", "UltimaActualizacion", "UltimaFalla", "UltimoMantenimiento" },
                values: new object[] { false, 0, new DateTime(2025, 8, 12, 19, 13, 2, 327, DateTimeKind.Utc).AddTicks(1676), 0, 1, 0, new DateTime(2025, 8, 12, 19, 13, 2, 327, DateTimeKind.Utc).AddTicks(1676), null, null });

            migrationBuilder.UpdateData(
                table: "Bombas",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "EsBombaReserva", "Estado", "FechaCreacion", "HorasOperacion", "Prioridad", "TipoFalla", "UltimaActualizacion", "UltimaFalla", "UltimoMantenimiento" },
                values: new object[] { false, 0, new DateTime(2025, 8, 12, 19, 13, 2, 327, DateTimeKind.Utc).AddTicks(1679), 0, 1, 0, new DateTime(2025, 8, 12, 19, 13, 2, 327, DateTimeKind.Utc).AddTicks(1679), null, null });

            migrationBuilder.UpdateData(
                table: "Bombas",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "EsBombaReserva", "Estado", "FechaCreacion", "HorasOperacion", "Prioridad", "TipoFalla", "UltimaActualizacion", "UltimaFalla", "UltimoMantenimiento" },
                values: new object[] { false, 0, new DateTime(2025, 8, 12, 19, 13, 2, 327, DateTimeKind.Utc).AddTicks(1681), 0, 1, 0, new DateTime(2025, 8, 12, 19, 13, 2, 327, DateTimeKind.Utc).AddTicks(1682), null, null });

            migrationBuilder.UpdateData(
                table: "Tanques",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "FechaCreacion", "UltimaActualizacion" },
                values: new object[] { new DateTime(2025, 8, 12, 19, 13, 2, 327, DateTimeKind.Utc).AddTicks(1792), new DateTime(2025, 8, 12, 19, 13, 2, 327, DateTimeKind.Utc).AddTicks(1792) });

            migrationBuilder.UpdateData(
                table: "Tanques",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "FechaCreacion", "UltimaActualizacion" },
                values: new object[] { new DateTime(2025, 8, 12, 19, 13, 2, 327, DateTimeKind.Utc).AddTicks(1794), new DateTime(2025, 8, 12, 19, 13, 2, 327, DateTimeKind.Utc).AddTicks(1795) });

            migrationBuilder.UpdateData(
                table: "Tanques",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "FechaCreacion", "UltimaActualizacion" },
                values: new object[] { new DateTime(2025, 8, 12, 19, 13, 2, 327, DateTimeKind.Utc).AddTicks(1796), new DateTime(2025, 8, 12, 19, 13, 2, 327, DateTimeKind.Utc).AddTicks(1797) });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EsBombaReserva",
                table: "Bombas");

            migrationBuilder.DropColumn(
                name: "Estado",
                table: "Bombas");

            migrationBuilder.DropColumn(
                name: "HorasOperacion",
                table: "Bombas");

            migrationBuilder.DropColumn(
                name: "Prioridad",
                table: "Bombas");

            migrationBuilder.DropColumn(
                name: "TipoFalla",
                table: "Bombas");

            migrationBuilder.DropColumn(
                name: "UltimaFalla",
                table: "Bombas");

            migrationBuilder.DropColumn(
                name: "UltimoMantenimiento",
                table: "Bombas");

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

            migrationBuilder.UpdateData(
                table: "Tanques",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "FechaCreacion", "UltimaActualizacion" },
                values: new object[] { new DateTime(2025, 8, 12, 18, 43, 12, 939, DateTimeKind.Utc).AddTicks(4871), new DateTime(2025, 8, 12, 18, 43, 12, 939, DateTimeKind.Utc).AddTicks(4872) });

            migrationBuilder.UpdateData(
                table: "Tanques",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "FechaCreacion", "UltimaActualizacion" },
                values: new object[] { new DateTime(2025, 8, 12, 18, 43, 12, 939, DateTimeKind.Utc).AddTicks(4873), new DateTime(2025, 8, 12, 18, 43, 12, 939, DateTimeKind.Utc).AddTicks(4874) });

            migrationBuilder.UpdateData(
                table: "Tanques",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "FechaCreacion", "UltimaActualizacion" },
                values: new object[] { new DateTime(2025, 8, 12, 18, 43, 12, 939, DateTimeKind.Utc).AddTicks(4876), new DateTime(2025, 8, 12, 18, 43, 12, 939, DateTimeKind.Utc).AddTicks(4876) });
        }
    }
}
