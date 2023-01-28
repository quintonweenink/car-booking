using Microsoft.EntityFrameworkCore;

namespace BookingService.Database;

public class BookingContext : DbContext
{
    protected override void OnConfiguring
        (DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseInMemoryDatabase(databaseName: "BookingDb");
    }
    
    public BookingContext(DbContextOptions<BookingContext> options) : base(options)
    {
    }
    public DbSet<Booking> Bookings { get; set; }
}