using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BookingService.Database;

public class Booking
{
    [Key] 
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid CarId { get; set; } 
    
    public DateTime StartDate { get; set; }
    
    public DateTime EndDate { get; set; }
}