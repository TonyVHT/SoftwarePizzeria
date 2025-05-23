using ItaliaPizza.Server.Settings;
using ItaliaPizza.Server.Services.Implementations;
using ItaliaPizza.Server.Services.Interfaces;
using ItaliaPizza.Server.Settings.URI;
using Microsoft.EntityFrameworkCore;
using Serilog;
using ItaliaPizza.Server.Repositories.Implementations;
using ItaliaPizza.Server.Repositories.Interfaces;

Log.Logger = new LoggerConfiguration()
            .WriteTo.Console()
            .WriteTo.File("logs/log.txt", rollingInterval: RollingInterval.Infinite, shared: true)
            .CreateLogger();

var builder = WebApplication.CreateBuilder(args);

var connectionString = ConnectionStringProvider.GetConnectionString();
if (string.IsNullOrEmpty(connectionString))
{
    throw new InvalidOperationException("No se encontró la cadena de conexión en variables de entorno.");
}

//Tony services

builder.Services.AddDbContext<ItaliaPizzaDbContext>(options =>
    options.UseSqlServer(connectionString));

builder.Services.AddScoped<IPlatilloRepository, PlatilloRepository>();
builder.Services.AddScoped<IPlatilloService, PlatilloService>();
builder.Services.AddScoped<IProductoRepository, ProductoRepository>();
builder.Services.AddScoped<IProductoService, ProductoService>();
builder.Services.AddScoped<ICategoriaProductoRepository, CategoriaProductoRepository>();
builder.Services.AddScoped<IIngredienteService, IngredienteService>();
builder.Services.AddScoped<IRecetaService, RecetaService>();
builder.Services.AddScoped<IRecetaRepository, RecetaRepository>();
builder.Services.AddScoped<ICategoriaProductoRepository, CategoriaProductoRepository>();
builder.Services.AddScoped<ICategoriaProductoService, CategoriaProductoService>();
builder.Services.AddControllers();
builder.Services.AddAuthorization();
builder.Services.AddScoped<IIngredienteRepository, IngredienteRepository>();
builder.Services.AddScoped<IProvedorService, ProveedorService>();
builder.Services.AddScoped<IProveedorRepository, ProveedorRepository>();
builder.Services.AddScoped<IMermaService, MermaService>();
builder.Services.AddScoped<IMermaRepository, MermaRepository>();
builder.Services.AddScoped<IMotivoMermaRepository, MotivoMermaRepository>();
builder.Services.AddScoped<IUsuarioService, UsuarioService>();
builder.Services.AddScoped<IUsuarioRepository, UsuarioRepository>();
builder.Services.AddScoped<ICredencialUsuarioService, CredencialUsuarioService>();
builder.Services.AddScoped<ICredencialUsuarioRepository,  CredencialUsuarioRepository>();
builder.Services.AddScoped<IPedidoProveedorService, PedidoProveedorService>();
builder.Services.AddScoped<IPedidoProveedorRepository, PedidoProveedorRepository>();
builder.Services.AddScoped<IPedidoService, PedidoService>();
builder.Services.AddScoped<IPedidoRepository, PedidoRepository>();
builder.Services.AddScoped<IPedidoDomicilioRepository, PedidoDomicilioRepository>();
builder.Services.AddScoped<IDetallePedidoRepository, DetallePedidoRepository>();
builder.Services.AddScoped<IPedidoLocalRepository, PedidoLocalRepository>();
builder.Services.AddScoped<IFinanzaRepository, FinanzaRepository>();
builder.Services.AddScoped<IClienteService, ClienteService>();
builder.Services.AddScoped<IClienteRepository, ClienteRepository>();
builder.Services.AddScoped<IDireccionClienteService, DireccionClienteService>();
builder.Services.AddScoped<IDireccionClienteRepository, DireccionClienteRepository>();
builder.Services.AddScoped<IProductoProveedorRepository, ProductoProveedorRepository>();
builder.Services.AddScoped<IProductoProveedorService, ProductoProveedorService>();
builder.Services.AddScoped<IFinanzaRepository, FinanzaRepository>();
builder.Services.AddScoped<IFinanzaService, FinanzaService>();
builder.Services.AddOpenApi();

var app = builder.Build();
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
