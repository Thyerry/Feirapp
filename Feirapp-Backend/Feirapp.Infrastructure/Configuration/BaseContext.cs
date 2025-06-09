using Feirapp.Entities.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

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
    
    [DbFunction("RAND")]
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
                .HasMany(e => e.PriceLogs)
                .WithOne(e => e.Store);
            entity
                .HasIndex(e => e.Cnpj)
                .IsUnique();
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
                .HasMany(e => e.PriceHistory)
                .WithOne(e => e.GroceryItem);
            entity
                .HasOne(e => e.Ncm)
                .WithMany()
                .HasForeignKey(e => e.NcmCode);
            entity
                .HasOne(e => e.Cest)
                .WithMany()
                .HasForeignKey(e => e.CestCode);
            entity
                .HasIndex(e => e.Barcode);
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
                .HasPrecision(10,2)
                .IsRequired();
            entity
                .Property(e => e.LogDate)
                .IsRequired();
            entity
                .Property(e => e.ProductCode)
                .HasMaxLength(20);
            entity
                .HasIndex(e => new { e.GroceryItemId, e.Barcode, e.LogDate, e.StoreId })
                .IsUnique();
        });

        modelBuilder.Entity<Ncm>(entity =>
        {
            entity
                .HasKey(e => e.Code);
            entity
                .HasMany(e => e.Cests)
                .WithOne(e => e.Ncm)
                .HasForeignKey(e => e.NcmCode);
        });

        modelBuilder.Entity<Cest>(entity =>
        {
            entity
                .HasKey(e => e.Code);
            entity
                .HasMany(e => e.GroceryItems)
                .WithOne(e => e.Cest)
                .HasForeignKey(e => e.CestCode);
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
                .HasColumnType("char(50)")
                .IsRequired();
            entity
                .Property(e => e.PasswordSalt)
                .HasColumnType("char(50)");
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
                .IsRequired();
            entity
                .Property(e => e.Url)
                .HasMaxLength(1024)
                .IsRequired();
            entity
                .HasOne(e => e.User)
                .WithMany()
                .HasForeignKey(e => e.UserId);
        });

        modelBuilder
            .HasDbFunction(typeof(BaseContext).GetMethod(nameof(Random), []) ?? throw new InvalidOperationException())
            .HasName("RAND");
    }
}