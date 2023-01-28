using Microsoft.EntityFrameworkCore;

namespace PriceService.Database;

public class PriceContext : DbContext
{
    protected override void OnConfiguring
        (DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseInMemoryDatabase(databaseName: "PriceDb");
    }
    
    public PriceContext(DbContextOptions<PriceContext> options) : base(options)
    {
    }
    public DbSet<Car> Cars { get; set; }
}