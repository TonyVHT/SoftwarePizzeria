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

builder.Services.AddScoped<IPlatilloService, PlatilloService>();
builder.Services.AddScoped<IProductoService, ProductoService>();

builder.Services.AddScoped<IPlatilloRepository, PlatilloRepository>();
builder.Services.AddScoped<IProductoRepository, ProductoRepository>();
builder.Services.AddScoped<ICategoriaProductoRepository, CategoriaProductoRepository>();

////
builder.Services.AddControllers();
builder.Services.AddAuthorization();
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
