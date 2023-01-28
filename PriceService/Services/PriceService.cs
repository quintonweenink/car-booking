using CarBooking.Controllers;
using Microsoft.EntityFrameworkCore;
using PricingService.Database;

namespace CarBooking;

public class PriceService
{
    private readonly ILogger<PriceService> _logger;
    private readonly PriceContext _priceContext;

    public PriceService(ILogger<PriceService> logger, PriceContext priceContext)
    {
        _logger = logger;
        _priceContext = priceContext;
    }

    public async Task<CarPriceResponse> GetPrice(Guid carId, DateTime startTime, DateTime endTime)
    {
        var carPrice = await _priceContext.Cars.FirstOrDefaultAsync(car => car.CarId.Equals(carId));

        return new CarPriceResponse()
        {
            FullPrice = GetPrice(carPrice.Price, startTime, endTime),
            CarId = carPrice.CarId
        };
    }
    
    private static decimal GetPrice(decimal price, DateTime startTime, DateTime endTime)
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