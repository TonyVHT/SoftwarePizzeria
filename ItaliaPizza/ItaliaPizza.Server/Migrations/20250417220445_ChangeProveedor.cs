using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ItaliaPizza.Server.Migrations
{
    /// <inheritdoc />
    public partial class ChangeProveedor : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Proveedores_Nombre",
                table: "Proveedores");

            migrationBuilder.RenameColumn(
                name: "Direccion",
                table: "Proveedores",
                newName: "Calle");

            migrationBuilder.AddColumn<string>(
                name: "ApellidoMaterno",
                table: "Proveedores",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ApellidoPaterno",
                table: "Proveedores",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "CodigoPostal",
                table: "Proveedores",
                type: "nvarchar(10)",
                maxLength: 10,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "NumeroDomicilio",
                table: "Proveedores",
                type: "nvarchar(10)",
                maxLength: 10,
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ApellidoMaterno",
                table: "Proveedores");

            migrationBuilder.DropColumn(
                name: "ApellidoPaterno",
                table: "Proveedores");

            migrationBuilder.DropColumn(
                name: "CodigoPostal",
                table: "Proveedores");

            migrationBuilder.DropColumn(
                name: "NumeroDomicilio",
                table: "Proveedores");

            migrationBuilder.RenameColumn(
                name: "Calle",
                table: "Proveedores",
                newName: "Direccion");

            migrationBuilder.CreateIndex(
                name: "IX_Proveedores_Nombre",
                table: "Proveedores",
                column: "Nombre",
                unique: true);
        }
    }
}
