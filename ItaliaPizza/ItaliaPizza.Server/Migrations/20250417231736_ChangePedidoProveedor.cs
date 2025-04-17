using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ItaliaPizza.Server.Migrations
{
    /// <inheritdoc />
    public partial class ChangePedidoProveedor : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PedidosProveedores_Usuarios_UsuarioRecibeId",
                table: "PedidosProveedores");

            migrationBuilder.DropForeignKey(
                name: "FK_PedidosProveedores_Usuarios_UsuarioSolicitaId",
                table: "PedidosProveedores");

            migrationBuilder.DropIndex(
                name: "IX_PedidosProveedores_UsuarioRecibeId",
                table: "PedidosProveedores");

            migrationBuilder.DropColumn(
                name: "UsuarioRecibeId",
                table: "PedidosProveedores");

            migrationBuilder.RenameColumn(
                name: "UsuarioSolicitaId",
                table: "PedidosProveedores",
                newName: "ProductoId");

            migrationBuilder.RenameColumn(
                name: "Estatus",
                table: "PedidosProveedores",
                newName: "EstadoDePedido");

            migrationBuilder.RenameIndex(
                name: "IX_PedidosProveedores_UsuarioSolicitaId",
                table: "PedidosProveedores",
                newName: "IX_PedidosProveedores_ProductoId");

            migrationBuilder.AddColumn<decimal>(
                name: "Cantidad",
                table: "PedidosProveedores",
                type: "decimal(10,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<bool>(
                name: "EstadoEliminacion",
                table: "PedidosProveedores",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "FechaLlegada",
                table: "PedidosProveedores",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UsuarioRecibe",
                table: "PedidosProveedores",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UsuarioSolicita",
                table: "PedidosProveedores",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddForeignKey(
                name: "FK_PedidosProveedores_Productos_ProductoId",
                table: "PedidosProveedores",
                column: "ProductoId",
                principalTable: "Productos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PedidosProveedores_Productos_ProductoId",
                table: "PedidosProveedores");

            migrationBuilder.DropColumn(
                name: "Cantidad",
                table: "PedidosProveedores");

            migrationBuilder.DropColumn(
                name: "EstadoEliminacion",
                table: "PedidosProveedores");

            migrationBuilder.DropColumn(
                name: "FechaLlegada",
                table: "PedidosProveedores");

            migrationBuilder.DropColumn(
                name: "UsuarioRecibe",
                table: "PedidosProveedores");

            migrationBuilder.DropColumn(
                name: "UsuarioSolicita",
                table: "PedidosProveedores");

            migrationBuilder.RenameColumn(
                name: "ProductoId",
                table: "PedidosProveedores",
                newName: "UsuarioSolicitaId");

            migrationBuilder.RenameColumn(
                name: "EstadoDePedido",
                table: "PedidosProveedores",
                newName: "Estatus");

            migrationBuilder.RenameIndex(
                name: "IX_PedidosProveedores_ProductoId",
                table: "PedidosProveedores",
                newName: "IX_PedidosProveedores_UsuarioSolicitaId");

            migrationBuilder.AddColumn<int>(
                name: "UsuarioRecibeId",
                table: "PedidosProveedores",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_PedidosProveedores_UsuarioRecibeId",
                table: "PedidosProveedores",
                column: "UsuarioRecibeId");

            migrationBuilder.AddForeignKey(
                name: "FK_PedidosProveedores_Usuarios_UsuarioRecibeId",
                table: "PedidosProveedores",
                column: "UsuarioRecibeId",
                principalTable: "Usuarios",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_PedidosProveedores_Usuarios_UsuarioSolicitaId",
                table: "PedidosProveedores",
                column: "UsuarioSolicitaId",
                principalTable: "Usuarios",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
