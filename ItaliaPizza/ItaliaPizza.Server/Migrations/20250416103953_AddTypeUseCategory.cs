using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ItaliaPizza.Server.Migrations
{
    /// <inheritdoc />
    public partial class AddTypeUseCategory : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "TipoDeUso",
                table: "CategoriasProductos",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TipoDeUso",
                table: "CategoriasProductos");
        }
    }
}
