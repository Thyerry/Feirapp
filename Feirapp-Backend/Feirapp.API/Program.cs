using Feirapp.API.Helpers;
using Feirapp.DAL.DataContext;
using Feirapp.DAL.Repositories;
using Feirapp.Domain.Contracts;
using Feirapp.Service.Services;
using MongoDB.Driver;

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


{
    app.UseCors(x => x
        .AllowAnyOrigin()
        .AllowAnyMethod()
        .AllowAnyHeader());
    
    // global exception handler
    app.UseMiddleware<ExceptionHandlerMiddleware>();
    
    app.UseHttpsRedirection();

    //app.UseAuthorization();

    app.MapControllers();

    app.Run();
}


void ConfigurationAndServices(IServiceCollection services, IConfiguration configuration)
{
    services.Configure<MongoSettings>(configuration.GetSection(nameof(MongoSettings)));
    services.AddTransient<IMongoFeirappContext, MongoFeirappContext>();
    services.AddTransient<IGroceryItemService, GroceryItemService>();
    services.AddTransient<IGroceryItemRepository, GroceryItemRepository>();
}
