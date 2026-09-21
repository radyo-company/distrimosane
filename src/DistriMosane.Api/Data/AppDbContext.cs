using DistriMosane.Api.Domain;
using Microsoft.EntityFrameworkCore;

namespace DistriMosane.Api.Data;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<Customer> Customers => Set<Customer>();

    public DbSet<Order> Orders => Set<Order>();

    public DbSet<OrderLine> OrderLines => Set<OrderLine>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Customer>(entity =>
        {
            entity.Property(c => c.CompanyName).IsRequired().HasMaxLength(200);
            entity.Property(c => c.VatNumber).IsRequired().HasMaxLength(20);
            entity.Property(c => c.Email).IsRequired().HasMaxLength(200);
            entity.Property(c => c.City).IsRequired().HasMaxLength(100);

            entity.HasIndex(c => c.VatNumber).IsUnique();
            entity.HasIndex(c => c.CompanyName);

            entity.HasMany(c => c.Orders)
                .WithOne(o => o.Customer!)
                .HasForeignKey(o => o.CustomerId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<Order>(entity =>
        {
            entity.Property(o => o.Status).IsRequired().HasMaxLength(20).HasConversion<string>();

            entity.HasIndex(o => o.OrderDate);

            entity.HasMany(o => o.Lines)
                .WithOne(l => l.Order!)
                .HasForeignKey(l => l.OrderId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<OrderLine>(entity =>
        {
            entity.Property(l => l.ProductLabel).IsRequired().HasMaxLength(200);

            // SQLite ne dispose pas d'un type natif pour decimal.
            entity.Property(l => l.UnitPrice).HasConversion<double>();
        });
    }
}
