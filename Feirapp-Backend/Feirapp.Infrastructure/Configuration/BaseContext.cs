using Feirapp.Entities.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Feirapp.Infrastructure.Configuration;

public partial class BaseContext : DbContext
{
    public BaseContext(DbContextOptions options) : base(options)
    {
    }

    public DbSet<GroceryItem> GroceryItems { get; set; }
    public DbSet<PriceLog?> PriceLogs { get; set; }
    public DbSet<Store> Stores { get; set; }
    public DbSet<Ncm> Ncms { get; set; }
    public DbSet<Cest> Cests { get; set; }

    
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
            entity.HasMany(e => e.PriceHistory).WithOne(e => e.GroceryItem);
            entity.HasOne(e => e.Ncm).WithMany().HasForeignKey(e => e.NcmCode);
            entity.HasOne(e => e.Cest).WithMany().HasForeignKey(e => e.CestCode);
            entity.HasIndex(e => e.Barcode);
            entity.Property(e => e.MeasureUnit).IsRequired().HasConversion<string>();
        });

        modelBuilder.Entity<PriceLog>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Price).IsRequired();
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

        modelBuilder
            .HasDbFunction(typeof(BaseContext).GetMethod(nameof(Random), new Type[]{}) ?? throw new InvalidOperationException())
            .HasName("RAND");
    }
}

public partial class BaseContext 
{

}