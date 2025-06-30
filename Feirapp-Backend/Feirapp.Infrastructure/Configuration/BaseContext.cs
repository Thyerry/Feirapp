using Feirapp.Entities.Entities;
using Feirapp.Infrastructure.Extensions;
using Microsoft.EntityFrameworkCore;

namespace Feirapp.Infrastructure.Configuration;

public class BaseContext(DbContextOptions options) : DbContext(options)
{
    public DbSet<GroceryItem> GroceryItems { get; set; }
    public DbSet<PriceLog> PriceLogs { get; set; }
    public DbSet<Store> Stores { get; set; }
    public DbSet<Ncm> Ncms { get; set; }
    public DbSet<Cest> Cests { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<Invoice> Invoices { get; set; }
    
    [DbFunction("random", IsBuiltIn = true)]
    public static double Random()
    {
        throw new NotImplementedException();
    }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Store>(entity =>
        {
            entity
                .HasKey(e => e.Id);
            entity
                .Property(e => e.Name)
                .HasMaxLength(1024)
                .IsRequired();
            entity
                .Property(e => e.AltNames)
                .HasColumnType("text[]");

            entity
                .Property(e => e.State)
                .HasConversion<string>()
                .HasMaxLength(2);
            entity
                .Property(e => e.Cep)
                .HasMaxLength(8);
            entity
                .Property(e => e.Street)
                .HasMaxLength(1024);
            entity
                .Property(e => e.StreetNumber)
                .HasMaxLength(20);
            entity
                .Property(e => e.Neighborhood)
                .HasMaxLength(1024);
            entity
                .Property(e => e.CityName)
                .HasMaxLength(1024);
            
            entity
                .HasIndex(e => e.Cnpj)
                .IsUnique();
            entity
                .HasIndex(e => e.AltNames)
                .HasMethod("GIN");
            entity
                .HasIndex(e => e.Name);
            entity
                .HasIndex(e => e.CityName);
        });

        modelBuilder.Entity<GroceryItem>(entity =>
        {
            entity
                .HasKey(e => e.Id);
            entity
                .Property(e => e.Name)
                .IsRequired()
                .HasMaxLength(1024);
            entity
                .Property(e => e.Brand)
                .HasMaxLength(1024);
            entity
                .Property(e => e.ImageUrl)
                .HasMaxLength(1024);
            entity
                .HasIndex(e => e.Barcode);
            entity
                .HasIndex(e => e.Name);
            entity
                .Property(e => e.MeasureUnit)
                .IsRequired()
                .HasConversion<string>()
                .HasMaxLength(5);
        });

        modelBuilder.Entity<PriceLog>(entity =>
        {
            entity
                .HasKey(e => e.Id);
            entity
                .Property(e => e.Price)
                .HasPrecision(10, 2)
                .IsRequired();
            entity
                .Property(e => e.LogDate)
                .IsRequired();
            entity
                .Property(e => e.ProductCode)
                .HasMaxLength(20);
            entity.HasIndex(e => e.GroceryItemId);
            entity.HasIndex(e => e.Barcode);
            entity.HasIndex(e => e.StoreId);
            entity.HasIndex(e => e.InvoiceId);
            entity.HasIndex(e => new { e.GroceryItemId, e.Barcode, e.LogDate, e.StoreId }).IsUnique();
            entity.HasIndex(e => e.LogDate).HasDatabaseName("idx_pricelogs_logdate_desc");
        });

        modelBuilder.Entity<Ncm>(entity =>
        {
            entity.HasKey(e => e.Code);
        });

        modelBuilder.Entity<Cest>(entity =>
        {
            entity.HasKey(e => e.Code);
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity
                .HasKey(e => e.Id);
            entity
                .Property(e => e.Name)
                .HasMaxLength(1024);
            entity
                .Property(e => e.Email)
                .HasMaxLength(256)
                .IsRequired();
            entity
                .Property(e => e.Password)
                .HasMaxLength(50)
                .IsFixedLength();
            entity
                .Property(e => e.PasswordSalt)
                .HasMaxLength(50);
            entity
                .Property(e => e.Status)
                .HasConversion<string>()
                .HasMaxLength(20);
        });
        
        modelBuilder.Entity<Invoice>(entity =>
        {
            entity
                .HasKey(e => e.Id);
            entity
                .Property(e => e.Code)
                .HasMaxLength(256)
                .IsRequired();
            entity
                .Property(e => e.ScanDate)
                .HasColumnType("timestamp with time zone")
                .IsRequired();
            entity
                .Property(e => e.Url)
                .HasMaxLength(1024)
                .IsRequired();
            entity.HasIndex(e => e.Code);
            entity.HasIndex(e => e.ScanDate);
        });

        modelBuilder
            .HasDbFunction(typeof(BaseContext).GetMethod(nameof(Random), []) ?? throw new InvalidOperationException())
            .HasName("RANDOM");
        
        foreach (var entityType in modelBuilder.Model.GetEntityTypes())
        {
            var properties = entityType.ClrType.GetProperties()
                .Where(p => p.PropertyType == typeof(DateTime));

            foreach (var prop in properties)
            {
                modelBuilder.Entity(entityType.ClrType)
                    .Property(prop.Name)
                    .HasConversion(new UtcDateTimeConverter());
            }
        }
    }
}