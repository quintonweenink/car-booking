using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PricingService.Database;

namespace CarBooking.Controllers;

[ApiController]
[Route("[controller]")]
public class PriceController : ControllerBase
{
    private readonly ILogger<PriceController> _logger;
    private readonly PriceContext _priceContext;

    public PriceController(ILogger<PriceController> logger, PriceContext priceContext)
    {
        _logger = logger;
        _priceContext = priceContext;
        _priceContext.SaveChanges();
    }

    [HttpGet(Name = "GetCarPriceAndAvailability")]
    public async Task<Response.ResultResponse<CarPriceResponse>> Get(
        [FromQuery] Guid carId, 
        [FromQuery] DateTime startTime, 
        [FromQuery] DateTime endTime)
    {
        var carPrice = await _priceContext.Cars.FirstOrDefaultAsync(car => car.CarId.Equals(carId));
        if (carPrice == null)
        {
            return new Response.ResultResponse<CarPriceResponse>()
            {
                Success = false,
                Message = $"Car with identifier {carId.ToString()} not found."
            };
        }
        
        return new Response.ResultResponse<CarPriceResponse>
        {
            Success = true,
            Result = new CarPriceResponse()
            {
                FullPrice = PriceService.getPrice(carPrice.Price, startTime, endTime),
                CarId = carPrice.CarId
            }
        };

    }
}