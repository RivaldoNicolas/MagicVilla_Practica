using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace MagicVillaAPI.Migrations
{
    /// <inheritdoc />
    public partial class AlimentarTablaVilla : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Villas",
                columns: new[] { "Id", "Amenidad", "Detalle", "FechaActualizacion", "FechaCreacion", "ImagenUrl", "MetrosCuadrados", "Nombre", "Ocupantes", "Tarifa" },
                values: new object[,]
                {
                    { 1, "", "detalle villa", new DateTime(2024, 7, 9, 10, 2, 22, 908, DateTimeKind.Local).AddTicks(3468), new DateTime(2024, 7, 9, 10, 2, 22, 908, DateTimeKind.Local).AddTicks(3456), "", 50, "Villa Real", 5, 200.0 },
                    { 2, "", "detalle villa", new DateTime(2024, 7, 9, 10, 2, 22, 908, DateTimeKind.Local).AddTicks(3471), new DateTime(2024, 7, 9, 10, 2, 22, 908, DateTimeKind.Local).AddTicks(3470), "", 80, "Premiun villa", 4, 150.0 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Villas",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Villas",
                keyColumn: "Id",
                keyValue: 2);
        }
    }
}
