namespace BookingService.Controllers;

public class BookingResponse
{
    public Guid CarId { get; set; }
    public bool Available { get; set; }
}