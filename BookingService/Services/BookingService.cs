using BookingService.Controllers;
using BookingService.Database;
using Microsoft.EntityFrameworkCore;

namespace BookingService.Services;

public class BookingService : IBookingService
{
    private readonly ILogger<BookingService> _logger;
    private readonly BookingContext _bookingContext;

    public BookingService(ILogger<BookingService> logger, BookingContext bookingContext)
    {
        _logger = logger;
        _bookingContext = bookingContext;
    }

    public async Task<BookingResponse> IsCarAvailable(Guid carId, DateTime startTime, DateTime endTime)
    {
        var startDate = startTime.Date;
        var endDate = endTime.Date;
        var foundCarBooking = await _bookingContext.Bookings
            .AnyAsync(booking => booking.CarId.Equals(carId) && (
                (startDate >= booking.StartDate.Date && startDate <= booking.EndDate.Date) ||
                (endDate >= booking.StartDate.Date && endDate <= booking.EndDate.Date) ||
                (startDate <= booking.StartDate.Date && endDate >= booking.EndDate.Date) 
                ));
        return new BookingResponse()
        {
            Available = !foundCarBooking,
            CarId = carId
        };
    }
}