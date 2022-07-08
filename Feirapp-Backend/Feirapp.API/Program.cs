using Feirapp.Domain.Contracts;
using Feirapp.Service.Services;
using MongoDB.Driver;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

ConfigureServices(builder.Services, builder.Configuration);
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

void ConfigureServices(IServiceCollection services, ConfigurationManager configuration)
{
    services.AddTransient<IGroceryItemService, GroceryItemService>();
    services.Configure<DatabaseConfiguration>(configuration.GetSection(nameof(DatabaseConfiguration)));
}
