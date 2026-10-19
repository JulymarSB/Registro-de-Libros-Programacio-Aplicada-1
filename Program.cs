using RegistroLibros.Components;
using RegistroLibros.Context;
using RegistroLibros.Services;
using Microsoft.EntityFrameworkCore;
using Blazored.Toast;
using RegistroLibros.Models;

var builder = WebApplication.CreateBuilder(args);
var ConStr = builder.Configuration.GetConnectionString("ConStr");
builder.Services.AddDbContextFactory<Contexto>(opciones => opciones.UseSqlServer(ConStr));
builder.Services.AddRazorComponents().AddInteractiveServerComponents();
builder.Services.AddScoped<LibroService>();
builder.Services.AddScoped<EstudiantesService>();
builder.Services.AddScoped<PrestamoService>();
builder.Services.AddScoped<EntradaServices>();
builder.Services.AddBlazoredToast();
// Add services to the container.
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
app.UseStatusCodePagesWithReExecute("/not-found", createScopeForStatusCodePages: true);
app.UseHttpsRedirection();

app.UseAntiforgery();

app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
