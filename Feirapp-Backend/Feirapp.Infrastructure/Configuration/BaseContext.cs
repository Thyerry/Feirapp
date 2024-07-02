using Feirapp.Entities.Entities;
using Microsoft.EntityFrameworkCore;

namespace Feirapp.Infrastructure.Configuration;

public class BaseContext : DbContext
{
    public BaseContext(DbContextOptions options) : base(options)
    {
    }

    public DbSet<GroceryItem> GroceryItems { get; set; }
    public DbSet<PriceLog> PriceLogs { get; set; }
    public DbSet<Store> Stores { get; set; }
    public DbSet<Ncm> Ncms { get; set; }
    public DbSet<Cest> Cests { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Store>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Name).IsRequired();
            entity.Property(e => e.State).HasConversion<string>();
            entity.HasMany(e => e.PriceLogs).WithOne(e => e.Store);
            entity.HasIndex(e => e.Cnpj).IsUnique();
        });

        modelBuilder.Entity<GroceryItem>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Name).IsRequired();
            entity.Property(e => e.Price).HasPrecision(2);
            entity.Property(e => e.LastPurchaseDate).IsRequired();
            entity.HasMany(e => e.PriceHistory).WithOne(e => e.GroceryItem);
            entity.HasOne(e => e.Ncm).WithMany().HasForeignKey(e => e.NcmCode);
            entity.HasOne(e => e.Cest).WithMany().HasForeignKey(e => e.CestCode);
            entity.HasIndex(e => e.Barcode);
            entity.Property(e => e.MeasureUnit).IsRequired().HasConversion<string>();
        });

        modelBuilder.Entity<PriceLog>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Price).IsRequired().HasPrecision(2);
            entity.Property(e => e.LogDate).IsRequired();
            entity.HasIndex(e => new { e.GroceryItemId, e.Barcode, e.LogDate, e.StoreId }).IsUnique();
        });

        modelBuilder.Entity<Ncm>(entity =>
        {
            entity.HasKey(e => e.Code);
            entity.HasMany(e => e.Cests).WithOne(e => e.Ncm).HasForeignKey(e => e.NcmCode);
        });

        modelBuilder.Entity<Cest>(entity =>
        {
            entity.HasKey(e => e.Code);
            entity.HasMany(e => e.GroceryItems).WithOne(e => e.Cest).HasForeignKey(e => e.CestCode);
        });
    }
}