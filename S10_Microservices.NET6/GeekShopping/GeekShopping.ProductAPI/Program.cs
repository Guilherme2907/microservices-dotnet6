using GeekShopping.ProductAPI.Configuration;
using GeekShopping.ProductAPI.Repository;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


/// <summary>
/// EF context configuration
/// </summary>
builder.Services.ConfigureSqlServerContext(builder.Configuration);

/// <summary>
/// AutoMpper configuration
/// </summary>
builder.Services.ConfigureAutoMapper();

/// <summary>
/// Dependency Injection
/// </summary>
builder.Services.AddScoped<IProductRepository, ProductRepository>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors();

app.UseAuthorization();

app.MapControllers();

app.Run();
