using System.Diagnostics;
using System.Globalization;
using System.Text;
using Feirapp.API.Helpers;
using Feirapp.Domain.Services.DataScrapper.Implementations;
using Feirapp.Domain.Services.DataScrapper.Interfaces;
using Feirapp.Domain.Services.GroceryItems.Implementations;
using Feirapp.Domain.Services.GroceryItems.Interfaces;
using Feirapp.Domain.Services.Stores.Implementations;
using Feirapp.Domain.Services.Stores.Interfaces;
using Feirapp.Domain.Services.UnitOfWork;
using Feirapp.Domain.Services.Users.Implementations;
using Feirapp.Domain.Services.Users.Interfaces;
using Feirapp.Entities.Enums;
using Feirapp.Infrastructure.Configuration;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

#region DB Context Configuration

builder.Services.AddDbContext<BaseContext>(options =>
{
    var pgsql = builder.Configuration.GetConnectionString("PostgresConnection");
    options.UseNpgsql(pgsql);
});

#endregion DB Context Configuration

DependencyInjection(builder.Services);

var jwtSettings = builder.Configuration.GetSection("JwtSettings");
var secretKey = Encoding.UTF8.GetBytes(jwtSettings["SecretKey"] ?? throw new InvalidOperationException("Secret key not found."));

var cultureInfo = new CultureInfo("pt-BR");
CultureInfo.DefaultThreadCurrentCulture = cultureInfo;
CultureInfo.DefaultThreadCurrentUICulture = cultureInfo;

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
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Feirapp", Version = "v1" });

    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Por favor, insira o token JWT no formato: Bearer {seu token}",
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement()
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                },
                Scheme = "oauth2",
                Name = "Bearer",
                In = ParameterLocation.Header,
            },
            new List<string>()
        }
    });
    
    c.EnableAnnotations();
    
    c.MapType<StatesEnum>(() => new OpenApiSchema
    {
        Type = "string",
        Enum = Enum.GetNames(typeof(StatesEnum))
            .Select(name => new Microsoft.OpenApi.Any.OpenApiString(name))
            .Cast<Microsoft.OpenApi.Any.IOpenApiAny>()
            .ToList()
    });
});


var app = builder.Build();

ApplyMigrations(app);

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

if (!Debugger.IsAttached)
{
    app.Urls.Add("http://0.0.0.0:8080");
}

app.UseAuthentication();
app.UseAuthorization();

app.UseMiddleware<ExceptionHandlerMiddleware>();

app.MapControllers();

app.Run();
return;

void DependencyInjection(IServiceCollection services)
{
    #region Services

    services.AddScoped<IUnitOfWork, UnitOfWork>();
    services.AddScoped<IGroceryItemService, GroceryItemService>();
    services.AddScoped<IInvoiceReaderService, InvoiceReaderService>();
    services.AddScoped<INcmCestDataScrapper, NcmCestDataScrapper>();
    services.AddScoped<IStoreService, StoreService>();
    services.AddScoped<IUserService, UserService>();

    #endregion Services
}

void ApplyMigrations(IApplicationBuilder application)
{
    using var scope = application.ApplicationServices.CreateScope();
    var services = scope.ServiceProvider;

    using var context = services.GetRequiredService<BaseContext>();
    var logger = services.GetRequiredService<ILogger<Program>>();

    logger.LogDebug("Server: {Server}", context.Database.GetDbConnection().DataSource);
    logger.LogDebug("Connection String: {ConnectionString}", context.Database.GetConnectionString());
    logger.LogDebug("Database Provider: {Provider}", context.Database.ProviderName);
    logger.LogDebug("Database Name: {DatabaseName}", context.Database.GetDbConnection().Database);
    
    if (!context.Database.GetPendingMigrations().Any())
    {
        logger.LogDebug("No pending migrations.");
        return;
    }
    
    logger.LogDebug("Applying migrations...");
    context.Database.Migrate();
}