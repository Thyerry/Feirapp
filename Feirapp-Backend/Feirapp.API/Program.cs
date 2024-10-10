using System.Text;
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
using Feirapp.Domain.Services.Users.Implementations;
using Feirapp.Domain.Services.Users.Interfaces;
using Feirapp.Infrastructure.Configuration;
using Feirapp.Infrastructure.Repository;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

#region DB Context Configuration

builder.Services.AddDbContext<BaseContext>(options =>
    options.UseMySql(builder.Configuration.GetConnectionString("MySqlConnection"),
        new MySqlServerVersion(new Version(8, 3, 0))));

#endregion DB Context Configuration

DependencyInjection(builder.Services, builder.Configuration);

var jwtSettings = builder.Configuration.GetSection("JwtSettings");
var secretKey = Encoding.UTF8.GetBytes(jwtSettings["SecretKey"] ?? throw new InvalidOperationException("Secret key not found."));

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(option =>
{
    option.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = jwtSettings["Issuer"],
        ValidAudience = jwtSettings["Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(secretKey),
        ClockSkew = TimeSpan.Zero
    };
});


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

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.UseMiddleware<ExceptionHandlerMiddleware>();

app.MapControllers();

app.Run();
return;

void DependencyInjection(IServiceCollection services, IConfiguration configuration)
{
    #region Configurations

    services.Configure<SefazPE>(configuration.GetSection("DataScrappingResources:SefazPE"));

    #endregion Configurations

    #region Services

    services.AddScoped<IGroceryItemService, GroceryItemService>();
    services.AddScoped<IInvoiceReaderService, InvoiceReaderService>();
    services.AddScoped<INcmCestDataScrapper, NcmCestDataScrapper>();
    services.AddScoped<IStoreService, StoreService>();
    services.AddScoped<IUserService, UserService>();

    #endregion Services

    #region Repositories

    services.AddScoped<IGroceryItemRepository, GroceryItemRepository>();
    services.AddScoped<IStoreRepository, StoreRepository>();
    services.AddScoped<INcmRepository, NcmRepository>();
    services.AddScoped<ICestRepository, CestRepository>();
    services.AddScoped<IUserRepository, UserRepository>();

    #endregion Repositories
}