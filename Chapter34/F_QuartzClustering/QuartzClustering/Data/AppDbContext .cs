using Microsoft.EntityFrameworkCore;

namespace QuartzClustering.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions options) : base(options)
    {
    }

    public DbSet<ExchangeRates> ExchangeRates { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .Entity<ExchangeRateValues>()
            .Property(x=>x.Value)
            .HasPrecision(20, 12);
    }

}
