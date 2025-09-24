// Program.cs
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Reclutamiento.WebApp.Services;
using ReclutamientoFrontend.WebApp.Services;
using System;

var builder = WebApplication.CreateBuilder(args);

// Configuration
var configuration = builder.Configuration;
var apiBase = configuration["ApiBaseUrl"] ?? "http://localhost:5000/";

// Add services
builder.Services.AddControllersWithViews();

// Session & HttpContext accessor
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromHours(2);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});
builder.Services.AddHttpContextAccessor();

// Register the AuthHeaderHandler
builder.Services.AddTransient<AuthHeaderHandler>();

// Typed HttpClients for services (the AuthHeaderHandler will add the Bearer token if present)
builder.Services.AddHttpClient<UsuarioService>(client =>
{
    client.BaseAddress = new Uri(apiBase);
}).AddHttpMessageHandler<AuthHeaderHandler>();

builder.Services.AddHttpClient<VacanteService>(client =>
{
    client.BaseAddress = new Uri(apiBase);
}).AddHttpMessageHandler<AuthHeaderHandler>();

builder.Services.AddHttpClient<SolicitudService>(client =>
{
    client.BaseAddress = new Uri(apiBase);
}).AddHttpMessageHandler<AuthHeaderHandler>();

builder.Services.AddHttpClient<RespuestaService>(client =>
{
    client.BaseAddress = new Uri(apiBase);
}).AddHttpMessageHandler<AuthHeaderHandler>();

// Auth service (for login endpoints). It may not need the handler, but we can still attach it.
builder.Services.AddHttpClient<IAuthService, AuthService>(client =>
{
    client.BaseAddress = new Uri(apiBase);
}).AddHttpMessageHandler<AuthHeaderHandler>();

// Add other scoped/utility services if needed (e.g., ApiClient) here...

var app = builder.Build();

// Configure middleware
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}
app.UseStaticFiles();
app.UseRouting();
app.UseSession();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
