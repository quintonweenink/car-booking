namespace PriceService.Services;

public interface IBookingService
{
    Task<bool> IsCarAvailable(Guid carId, DateTime startTime, DateTime endTime);
}