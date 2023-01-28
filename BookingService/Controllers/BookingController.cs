using BookingService.Services;
using Microsoft.AspNetCore.Mvc;

namespace BookingService.Controllers;

[ApiController]
[Route("[controller]")]
public class BookingController : ControllerBase
{
    private readonly ILogger<BookingController> _logger;
    private readonly IBookingService _bookingService;

    public BookingController(ILogger<BookingController> logger, IBookingService bookingService)
    {
        _logger = logger;
        _bookingService = bookingService;
    }

    [HttpGet(Name = "GetCarAvailability")]
    public async Task<Response.ResultResponse<BookingResponse>> Get(
        [FromQuery] Guid carId, 
        [FromQuery] DateTime startTime, 
        [FromQuery] DateTime endTime)
    {
        try
        {
            return new Response.ResultResponse<BookingResponse>
            {
                Success = true,
                Result = await _bookingService.IsCarAvailable(carId, startTime, endTime)
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            return new Response.ResultResponse<BookingResponse>
            {
                Success = false,
                Message = ex.Message
            };
        }
    }
}