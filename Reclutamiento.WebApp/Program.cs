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
var apiBase = configuration["ApiBaseUrl"] ?? "http://localhost:5148/";

// Add services
builder.Services.AddControllersWithViews();

builder.Services.AddTransient<SolicitudService>();
builder.Services.AddTransient<VacanteService>();
builder.Services.AddTransient<UsuarioService>();

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
//configuraci�n de la autenticaci�n de la aplicaci�n usando cookies
builder.Services.AddAuthentication("AuthCookie")
.AddCookie("AuthCookie", options =>
{
    options.LoginPath = "/Auth/Login";   // Aqu� siempre envia a la vista login
    options.ExpireTimeSpan = TimeSpan.FromMinutes(60);
})
.AddGitHub(options =>
{
    options.ClientId = "Ov23li9wcxHhAXYkGvco";
    options.ClientSecret = "4a5d6036d1e5500c8671917fffe133555d4918e0";
    options.CallbackPath = new PathString("/auth/github-callback");
    options.SaveTokens = true;
});
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
    pattern: "{controller=Auth}/{action=Login}/{id?}");

app.Run();
