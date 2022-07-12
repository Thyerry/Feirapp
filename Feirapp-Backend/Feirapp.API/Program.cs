using Feirapp.DAL.DataContext;
using Feirapp.DAL.Repositories;
using Feirapp.Domain.Contracts;
using Feirapp.Domain.Models;
using Feirapp.Service.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

ConfigurationAndServices(builder.Services, builder.Configuration);
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

void ConfigurationAndServices(IServiceCollection services, IConfiguration configuration)
{
    services.Configure<MongoSettings>(configuration.GetSection(nameof(MongoSettings)));
    services.AddTransient<IMongoFeirappContext, MongoFeirappContext>();
    services.AddTransient<IGroceryItemService, GroceryItemService>();
    services.AddTransient<IGroceryItemRepository, GroceryItemRepository>();
}