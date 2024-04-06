using Feirapp.API.Helpers;
using Feirapp.Domain.Contracts.Repository;
using Feirapp.Domain.Contracts.Service;
using Feirapp.Domain.Services;
using Feirapp.Infrastructure.DataContext;
using Feirapp.Infrastructure.Repository;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

ConfigurationAndServices(builder.Services, builder.Configuration);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors(x => x
    .AllowAnyOrigin()
    .AllowAnyMethod()
    .AllowAnyHeader());

app.UseMiddleware<ExceptionHandlerMiddleware>();

//app.UseHttpsRedirection();

//app.UseAuthorization();

app.MapControllers();

app.Run();

void ConfigurationAndServices(IServiceCollection services, IConfiguration configuration)
{
    services.Configure<MongoSettings>(configuration.GetSection(nameof(MongoSettings)));
    services.AddTransient<IMongoFeirappContext, MongoFeirappContext>();

    #region Services

    services.AddTransient<IGroceryItemService, GroceryItemService>();
    services.AddTransient<IGroceryCategoryService, GroceryCategoryService>();

    #endregion Services

    #region Repositories

    services.AddTransient<IGroceryItemRepository, GroceryItemRepository>();
    services.AddTransient<IGroceryCategoryRepository, GroceryCategoryRepository>();

    #endregion Repositories
}