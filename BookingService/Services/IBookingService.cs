using BookingService.Controllers;

namespace BookingService.Services;

public interface IBookingService
{
    Task<BookingResponse> IsCarAvailable(Guid carId, DateTime startTime, DateTime endTime);
}