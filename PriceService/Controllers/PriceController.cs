using Microsoft.AspNetCore.Mvc;
using PriceService.Services;

namespace PriceService.Controllers;

[ApiController]
[Route("[controller]")]
public class PriceController : ControllerBase
{
    private readonly ILogger<PriceController> _logger;
    private readonly IPriceService _priceService;

    public PriceController(ILogger<PriceController> logger, IPriceService priceService)
    {
        _logger = logger;
        _priceService = priceService;
    }

    [HttpGet(Name = "GetCarPriceAndAvailability")]
    public async Task<Response.ResultResponse<CarPriceResponse>> Get(
        [FromQuery] Guid carId, 
        [FromQuery] DateTime startTime, 
        [FromQuery] DateTime endTime)
    {
        try
        {
            return new Response.ResultResponse<CarPriceResponse>
            {
                Success = true,
                Result = await _priceService.GetPrice(carId, startTime, endTime)
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            return new Response.ResultResponse<CarPriceResponse>
            {
                Success = false,
                Message = ex.Message
            };
        }
    }
}