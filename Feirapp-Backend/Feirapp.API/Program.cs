using Feirapp.API.Helpers;
using Feirapp.Domain.Services.Cests.Interfaces;
using Feirapp.Domain.Services.DataScrapper.Dtos;
using Feirapp.Domain.Services.DataScrapper.Implementations;
using Feirapp.Domain.Services.DataScrapper.Interfaces;
using Feirapp.Domain.Services.GroceryItems.Implementations;
using Feirapp.Domain.Services.GroceryItems.Interfaces;
using Feirapp.Domain.Services.Ncms.Interfaces;
using Feirapp.Domain.Services.Stores.Implementations;
using Feirapp.Domain.Services.Stores.Interfaces;
using Feirapp.Infrastructure.Configuration;
using Feirapp.Infrastructure.Repository;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

#region DB Context Configuration

builder.Services.AddDbContext<BaseContext>(options =>
    options.UseMySql(builder.Configuration.GetConnectionString("MySqlConnection"),
        new MySqlServerVersion(new Version(8, 3, 0))));

#endregion DB Context Configuration

ConfigurationsAndServices(builder.Services, builder.Configuration);

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
return;

void ConfigurationsAndServices(IServiceCollection services, IConfiguration configuration)
{
    #region Configurations

    services.Configure<SefazPE>(configuration.GetSection("DataScrappingResources:SefazPE"));

    #endregion Configurations

    #region Services

    services.AddScoped<IGroceryItemService, GroceryItemService>();
    services.AddScoped<IInvoiceReaderService, InvoiceReaderService>();
    services.AddScoped<INcmCestDataScrapper, NcmCestDataScrapper>();
    services.AddScoped<IStoreService, StoreService>();

    #endregion Services

    #region Repositories

    services.AddScoped<IGroceryItemRepository, GroceryItemRepository>();
    services.AddScoped<IStoreRepository, StoreRepository>();
    services.AddScoped<INcmRepository, NcmRepository>();
    services.AddScoped<ICestRepository, CestRepository>();

    #endregion Repositories
}