using Microsoft.EntityFrameworkCore;
using Scalar.AspNetCore;
using RegistroLibros.Context;
using RegistroLibros.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddOpenApi();

var ConStr = builder.Configuration.GetConnectionString("SqlConStr");
builder.Services.AddDbContextFactory<Contexto>(options => options.UseSqlServer(ConStr));

builder.Services.AddTransient<LibroService>();
builder.Services.AddTransient<EstudiantesService>();
builder.Services.AddTransient<PrestamoService>();
builder.Services.AddTransient<EntradaServices>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
