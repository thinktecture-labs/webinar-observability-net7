using Microsoft.EntityFrameworkCore;

namespace Thinktecture.Webinars.SampleApi.Entities;

public class SampleApiContext : DbContext
{
    public SampleApiContext(DbContextOptions<SampleApiContext> options) : base(options)
    {
    }

    public DbSet<Product> Products { get; set; }
    public DbSet<Category> Categories { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Product>()
            .HasMany(e => e.Categories)
            .WithMany(e => e.Products);
    }
}
