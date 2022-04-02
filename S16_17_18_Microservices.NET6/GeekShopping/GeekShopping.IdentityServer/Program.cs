using Duende.IdentityServer.Services;
using GeekShopping.IdentityServer.Configuration;
using GeekShopping.IdentityServer.Initializer;
using GeekShopping.IdentityServer.Services;
using Microsoft.Extensions.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

/// <summary>
/// EF context configuration
/// </summary>
builder.Services.ConfigureSqlServerContext(builder.Configuration);

/// <summary>
/// IdentityServer configuration
/// </summary>
builder.Services.ConfigureIdentityServer();

/// <summary>
/// Dependency injection
/// </summary>
builder.Services.AddScoped<IDbInitializer, DbInitializer>();
builder.Services.AddScoped<IProfileService, ProfileService>();

var app = builder.Build();
var scope = app.Services.CreateScope();

var dbInitializer = scope.ServiceProvider.GetService<IDbInitializer>(); 

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.UseIdentityServer();

app.UseAuthorization();

dbInitializer.Initialize();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
