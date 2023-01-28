namespace PriceService.Services;

public interface IPriceService
{
    Task<Controllers.CarPriceResponse> GetPrice(Guid carId, DateTime startTime, DateTime endTime);
}