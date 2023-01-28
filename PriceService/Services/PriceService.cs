using Microsoft.EntityFrameworkCore;
using PriceService.Database;

namespace PriceService.Services;

public class PriceService : IPriceService
{
    private readonly ILogger<PriceService> _logger;
    private readonly PriceContext _priceContext;
    private readonly IBookingService _bookingService;

    public PriceService(ILogger<PriceService> logger, PriceContext priceContext, IBookingService bookingService)
    {
        _logger = logger;
        _priceContext = priceContext;
        _bookingService = bookingService;
    }

    public async Task<Controllers.CarPriceResponse> GetPrice(Guid carId, DateTime startTime, DateTime endTime)
    {
        if(!await _bookingService.IsCarAvailable(carId, startTime, endTime))
        {
            _logger.LogError($"Car with identifier {carId.ToString()} has already been booked for this time");
            throw new Exception($"Car with identifier {carId.ToString()} has already been booked for this time"); 
        }
        
        var carPrice = await _priceContext.Cars.FirstOrDefaultAsync(car => car.CarId.Equals(carId));
        if (carPrice == null)
        {
            _logger.LogError($"Car with identifier {carId.ToString()} not found");
            throw new Exception($"Car with identifier {carId.ToString()} not found");
        }

        return new Controllers.CarPriceResponse()
        {
            FullPrice = CalculatePrice(carPrice.Price, startTime, endTime),
            CarId = carPrice.CarId
        };
    }
    
    private static decimal CalculatePrice(decimal price, DateTime startTime, DateTime endTime)
    {
        decimal totalPrice = 0;
        int days = 0;
        var tempDate = startTime.Date;
        var endDate = endTime.Date;
        while (tempDate <= endDate)
        {
            var dayPrice = price;
            if (tempDate.DayOfWeek.Equals(DayOfWeek.Saturday) ||
                tempDate.DayOfWeek.Equals(DayOfWeek.Sunday))
            {
                dayPrice *= (decimal)1.05;
            }
            totalPrice += dayPrice;
            days++;
            tempDate = tempDate.AddDays(1);
        }

        totalPrice += price * (decimal)0.1 * days; //Insurance TODO: Make this a setting
        totalPrice += price * (decimal)0.1 * days; //Snappcar TODO: Make this a setting
        
        if (days > 3) //Discount TODO: Make this a setting
        {
            totalPrice -= totalPrice * (decimal)0.15; //Discount TODO: Make this a setting
        }

        return totalPrice;
    }
}