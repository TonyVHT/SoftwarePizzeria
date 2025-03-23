using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ItaliaPizza.Server.Migrations
{
    /// <inheritdoc />
    public partial class PrimeraMigracion : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CategoriasPlatillos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Descripcion = table.Column<string>(type: "NVARCHAR(MAX)", nullable: true),
                    Estatus = table.Column<bool>(type: "bit", nullable: false, defaultValue: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CategoriasPlatillos", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CategoriasProductos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Estatus = table.Column<bool>(type: "bit", nullable: false, defaultValue: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CategoriasProductos", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Clientes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Apellidos = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Telefono = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Direccion = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    CodigoPostal = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    Ciudad = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Estatus = table.Column<bool>(type: "bit", nullable: false, defaultValue: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Clientes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "MotivosMermas",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Descripcion = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MotivosMermas", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Proveedores",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Telefono = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Direccion = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Ciudad = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Estatus = table.Column<bool>(type: "bit", nullable: false, defaultValue: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Proveedores", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Usuarios",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Apellidos = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Telefono = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Direccion = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    CodigoPostal = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    Ciudad = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Rol = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Estatus = table.Column<bool>(type: "bit", nullable: false, defaultValue: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Usuarios", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Platillos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    CodigoPlatillo = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    CategoriaId = table.Column<int>(type: "int", nullable: false),
                    Descripcion = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Precio = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    Foto = table.Column<byte[]>(type: "varbinary(max)", nullable: true),
                    Restriccion = table.Column<int>(type: "int", nullable: true),
                    Estatus = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    Instrucciones = table.Column<string>(type: "NVARCHAR(MAX)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Platillos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Platillos_CategoriasPlatillos_CategoriaId",
                        column: x => x.CategoriaId,
                        principalTable: "CategoriasPlatillos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Productos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    CategoriaId = table.Column<int>(type: "int", nullable: false),
                    ProveedorId = table.Column<int>(type: "int", nullable: false),
                    UnidadMedida = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    CantidadActual = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    CantidadMinima = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    Precio = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    Estatus = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    ObservacionesInventario = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Productos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Productos_CategoriasProductos_CategoriaId",
                        column: x => x.CategoriaId,
                        principalTable: "CategoriasProductos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Productos_Proveedores_ProveedorId",
                        column: x => x.ProveedorId,
                        principalTable: "Proveedores",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "CredencialesUsuarios",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UsuarioId = table.Column<int>(type: "int", nullable: false),
                    NombreUsuario = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    HashContraseña = table.Column<byte[]>(type: "varbinary(64)", nullable: false),
                    Salt = table.Column<byte[]>(type: "varbinary(32)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CredencialesUsuarios", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CredencialesUsuarios_Usuarios_UsuarioId",
                        column: x => x.UsuarioId,
                        principalTable: "Usuarios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Finanzas",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TipoTransaccion = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Concepto = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Monto = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    Fecha = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()"),
                    UsuarioId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Finanzas", x => x.Id);
                    table.CheckConstraint("CK_TipoTransaccion", "TipoTransaccion IN ('Entrada', 'Salida')");
                    table.ForeignKey(
                        name: "FK_Finanzas_Usuarios_UsuarioId",
                        column: x => x.UsuarioId,
                        principalTable: "Usuarios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PedidosProveedores",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProveedorId = table.Column<int>(type: "int", nullable: false),
                    FechaPedido = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()"),
                    Total = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    Estatus = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false, defaultValue: "Pendiente"),
                    UsuarioSolicitaId = table.Column<int>(type: "int", nullable: false),
                    UsuarioRecibeId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PedidosProveedores", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PedidosProveedores_Proveedores_ProveedorId",
                        column: x => x.ProveedorId,
                        principalTable: "Proveedores",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PedidosProveedores_Usuarios_UsuarioRecibeId",
                        column: x => x.UsuarioRecibeId,
                        principalTable: "Usuarios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PedidosProveedores_Usuarios_UsuarioSolicitaId",
                        column: x => x.UsuarioSolicitaId,
                        principalTable: "Usuarios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Ingredientes",
                columns: table => new
                {
                    IdProducto = table.Column<int>(type: "int", nullable: false),
                    CategoriaId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Ingredientes", x => x.IdProducto);
                    table.ForeignKey(
                        name: "FK_Ingredientes_CategoriasProductos_CategoriaId",
                        column: x => x.CategoriaId,
                        principalTable: "CategoriasProductos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Ingredientes_Productos_IdProducto",
                        column: x => x.IdProducto,
                        principalTable: "Productos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Pedidos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CajeroId = table.Column<int>(type: "int", nullable: false),
                    FechaPedido = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()"),
                    Total = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    MetodoPago = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Estatus = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false, defaultValue: "En proceso"),
                    TiempoPreparacion = table.Column<int>(type: "int", nullable: true),
                    TransaccionFinancieraId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Pedidos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Pedidos_Finanzas_TransaccionFinancieraId",
                        column: x => x.TransaccionFinancieraId,
                        principalTable: "Finanzas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Pedidos_Usuarios_CajeroId",
                        column: x => x.CajeroId,
                        principalTable: "Usuarios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "DetallesPedidoProveedores",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PedidoProveedorId = table.Column<int>(type: "int", nullable: false),
                    ProductoId = table.Column<int>(type: "int", nullable: false),
                    Cantidad = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    Subtotal = table.Column<decimal>(type: "decimal(10,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DetallesPedidoProveedores", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DetallesPedidoProveedores_PedidosProveedores_PedidoProveedorId",
                        column: x => x.PedidoProveedorId,
                        principalTable: "PedidosProveedores",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DetallesPedidoProveedores_Productos_ProductoId",
                        column: x => x.ProductoId,
                        principalTable: "Productos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Recetas",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PlatilloId = table.Column<int>(type: "int", nullable: false),
                    IngredienteId = table.Column<int>(type: "int", nullable: false),
                    Cantidad = table.Column<decimal>(type: "decimal(10,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Recetas", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Recetas_Ingredientes_IngredienteId",
                        column: x => x.IngredienteId,
                        principalTable: "Ingredientes",
                        principalColumn: "IdProducto",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Recetas_Platillos_PlatilloId",
                        column: x => x.PlatilloId,
                        principalTable: "Platillos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DetallesPedido",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PedidoId = table.Column<int>(type: "int", nullable: false),
                    PlatilloId = table.Column<int>(type: "int", nullable: false),
                    Cantidad = table.Column<int>(type: "int", nullable: false, defaultValue: 1),
                    Subtotal = table.Column<decimal>(type: "decimal(10,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DetallesPedido", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DetallesPedido_Pedidos_PedidoId",
                        column: x => x.PedidoId,
                        principalTable: "Pedidos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DetallesPedido_Platillos_PlatilloId",
                        column: x => x.PlatilloId,
                        principalTable: "Platillos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "HistorialEstatusPedidos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PedidoId = table.Column<int>(type: "int", nullable: false),
                    EstatusAnterior = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    EstatusNuevo = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    FechaCambio = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()"),
                    UsuarioId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HistorialEstatusPedidos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_HistorialEstatusPedidos_Pedidos_PedidoId",
                        column: x => x.PedidoId,
                        principalTable: "Pedidos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_HistorialEstatusPedidos_Usuarios_UsuarioId",
                        column: x => x.UsuarioId,
                        principalTable: "Usuarios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Mermas",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PedidoId = table.Column<int>(type: "int", nullable: true),
                    ProductoId = table.Column<int>(type: "int", nullable: false),
                    CantidadPerdida = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    MotivoMermaId = table.Column<int>(type: "int", nullable: false),
                    UsuarioId = table.Column<int>(type: "int", nullable: false),
                    Fecha = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Mermas", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Mermas_MotivosMermas_MotivoMermaId",
                        column: x => x.MotivoMermaId,
                        principalTable: "MotivosMermas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Mermas_Pedidos_PedidoId",
                        column: x => x.PedidoId,
                        principalTable: "Pedidos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Mermas_Productos_ProductoId",
                        column: x => x.ProductoId,
                        principalTable: "Productos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Mermas_Usuarios_UsuarioId",
                        column: x => x.UsuarioId,
                        principalTable: "Usuarios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PedidosDomicilio",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    ClienteId = table.Column<int>(type: "int", nullable: false),
                    DireccionEntrega = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Referencias = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    TelefonoContacto = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    RepartidorId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PedidosDomicilio", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PedidosDomicilio_Clientes_ClienteId",
                        column: x => x.ClienteId,
                        principalTable: "Clientes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PedidosDomicilio_Pedidos_Id",
                        column: x => x.Id,
                        principalTable: "Pedidos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PedidosDomicilio_Usuarios_RepartidorId",
                        column: x => x.RepartidorId,
                        principalTable: "Usuarios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PedidosLocal",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    NumeroMesa = table.Column<int>(type: "int", nullable: false),
                    MeseroId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PedidosLocal", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PedidosLocal_Pedidos_Id",
                        column: x => x.Id,
                        principalTable: "Pedidos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PedidosLocal_Usuarios_MeseroId",
                        column: x => x.MeseroId,
                        principalTable: "Usuarios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CategoriasPlatillos_Nombre",
                table: "CategoriasPlatillos",
                column: "Nombre",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CategoriasProductos_Nombre",
                table: "CategoriasProductos",
                column: "Nombre",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Clientes_Telefono",
                table: "Clientes",
                column: "Telefono",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CredencialesUsuarios_NombreUsuario",
                table: "CredencialesUsuarios",
                column: "NombreUsuario",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CredencialesUsuarios_UsuarioId",
                table: "CredencialesUsuarios",
                column: "UsuarioId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_DetallesPedido_PedidoId",
                table: "DetallesPedido",
                column: "PedidoId");

            migrationBuilder.CreateIndex(
                name: "IX_DetallesPedido_PlatilloId",
                table: "DetallesPedido",
                column: "PlatilloId");

            migrationBuilder.CreateIndex(
                name: "IX_DetallesPedidoProveedores_PedidoProveedorId",
                table: "DetallesPedidoProveedores",
                column: "PedidoProveedorId");

            migrationBuilder.CreateIndex(
                name: "IX_DetallesPedidoProveedores_ProductoId",
                table: "DetallesPedidoProveedores",
                column: "ProductoId");

            migrationBuilder.CreateIndex(
                name: "IX_Finanzas_UsuarioId",
                table: "Finanzas",
                column: "UsuarioId");

            migrationBuilder.CreateIndex(
                name: "IX_HistorialEstatusPedidos_PedidoId",
                table: "HistorialEstatusPedidos",
                column: "PedidoId");

            migrationBuilder.CreateIndex(
                name: "IX_HistorialEstatusPedidos_UsuarioId",
                table: "HistorialEstatusPedidos",
                column: "UsuarioId");

            migrationBuilder.CreateIndex(
                name: "IX_Ingredientes_CategoriaId",
                table: "Ingredientes",
                column: "CategoriaId");

            migrationBuilder.CreateIndex(
                name: "IX_Mermas_MotivoMermaId",
                table: "Mermas",
                column: "MotivoMermaId");

            migrationBuilder.CreateIndex(
                name: "IX_Mermas_PedidoId",
                table: "Mermas",
                column: "PedidoId");

            migrationBuilder.CreateIndex(
                name: "IX_Mermas_ProductoId",
                table: "Mermas",
                column: "ProductoId");

            migrationBuilder.CreateIndex(
                name: "IX_Mermas_UsuarioId",
                table: "Mermas",
                column: "UsuarioId");

            migrationBuilder.CreateIndex(
                name: "IX_MotivosMermas_Descripcion",
                table: "MotivosMermas",
                column: "Descripcion",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Pedidos_CajeroId",
                table: "Pedidos",
                column: "CajeroId");

            migrationBuilder.CreateIndex(
                name: "IX_Pedidos_TransaccionFinancieraId",
                table: "Pedidos",
                column: "TransaccionFinancieraId");

            migrationBuilder.CreateIndex(
                name: "IX_PedidosDomicilio_ClienteId",
                table: "PedidosDomicilio",
                column: "ClienteId");

            migrationBuilder.CreateIndex(
                name: "IX_PedidosDomicilio_RepartidorId",
                table: "PedidosDomicilio",
                column: "RepartidorId");

            migrationBuilder.CreateIndex(
                name: "IX_PedidosLocal_MeseroId",
                table: "PedidosLocal",
                column: "MeseroId");

            migrationBuilder.CreateIndex(
                name: "IX_PedidosProveedores_ProveedorId",
                table: "PedidosProveedores",
                column: "ProveedorId");

            migrationBuilder.CreateIndex(
                name: "IX_PedidosProveedores_UsuarioRecibeId",
                table: "PedidosProveedores",
                column: "UsuarioRecibeId");

            migrationBuilder.CreateIndex(
                name: "IX_PedidosProveedores_UsuarioSolicitaId",
                table: "PedidosProveedores",
                column: "UsuarioSolicitaId");

            migrationBuilder.CreateIndex(
                name: "IX_Platillos_CategoriaId",
                table: "Platillos",
                column: "CategoriaId");

            migrationBuilder.CreateIndex(
                name: "IX_Platillos_CodigoPlatillo",
                table: "Platillos",
                column: "CodigoPlatillo",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Platillos_Nombre",
                table: "Platillos",
                column: "Nombre",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Productos_CategoriaId",
                table: "Productos",
                column: "CategoriaId");

            migrationBuilder.CreateIndex(
                name: "IX_Productos_Nombre",
                table: "Productos",
                column: "Nombre",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Productos_ProveedorId",
                table: "Productos",
                column: "ProveedorId");

            migrationBuilder.CreateIndex(
                name: "IX_Proveedores_Nombre",
                table: "Proveedores",
                column: "Nombre",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Recetas_IngredienteId",
                table: "Recetas",
                column: "IngredienteId");

            migrationBuilder.CreateIndex(
                name: "IX_Recetas_PlatilloId",
                table: "Recetas",
                column: "PlatilloId");

            migrationBuilder.CreateIndex(
                name: "IX_Usuarios_Email",
                table: "Usuarios",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Usuarios_Telefono",
                table: "Usuarios",
                column: "Telefono",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CredencialesUsuarios");

            migrationBuilder.DropTable(
                name: "DetallesPedido");

            migrationBuilder.DropTable(
                name: "DetallesPedidoProveedores");

            migrationBuilder.DropTable(
                name: "HistorialEstatusPedidos");

            migrationBuilder.DropTable(
                name: "Mermas");

            migrationBuilder.DropTable(
                name: "PedidosDomicilio");

            migrationBuilder.DropTable(
                name: "PedidosLocal");

            migrationBuilder.DropTable(
                name: "Recetas");

            migrationBuilder.DropTable(
                name: "PedidosProveedores");

            migrationBuilder.DropTable(
                name: "MotivosMermas");

            migrationBuilder.DropTable(
                name: "Clientes");

            migrationBuilder.DropTable(
                name: "Pedidos");

            migrationBuilder.DropTable(
                name: "Ingredientes");

            migrationBuilder.DropTable(
                name: "Platillos");

            migrationBuilder.DropTable(
                name: "Finanzas");

            migrationBuilder.DropTable(
                name: "Productos");

            migrationBuilder.DropTable(
                name: "CategoriasPlatillos");

            migrationBuilder.DropTable(
                name: "Usuarios");

            migrationBuilder.DropTable(
                name: "CategoriasProductos");

            migrationBuilder.DropTable(
                name: "Proveedores");
        }
    }
}
