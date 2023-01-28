using System.Text;
using Newtonsoft.Json;
using PriceService.Controllers;

namespace PriceService.Services;

public class BookingService : IBookingService
{
    private readonly ILogger<PriceService> _logger;

    public BookingService(ILogger<PriceService> logger)
    {
        _logger = logger;
    }

    public async Task<bool> IsCarAvailable(Guid carId, DateTime startTime, DateTime endTime)
    {
        using var httpClient = new HttpClient();
        using var response = await httpClient.GetAsync($"http://localhost:5135/Booking?carId={carId.ToString()}&startTime={startTime}&endTime={endTime}");
        var apiResponse = await response.Content.ReadAsStringAsync();
        var receivedReservation = JsonConvert.DeserializeObject<Response.ResultResponse<BookingResponse>>(apiResponse);
        return receivedReservation.Result.Available;
    }
    
}