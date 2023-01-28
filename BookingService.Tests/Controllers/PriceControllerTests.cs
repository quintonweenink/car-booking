using BookingService.Controllers;
using BookingService.Database;
using FakeItEasy;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace PriceService.Tests.Controllers;

public class Tests
{
    private BookingController _bookingController;
    private BookingContext dbContext;
    private BookingService.Services.BookingService _bookingService;

    private const string datetimeTuesday = "2030-01-01T12:16:22.697099+02:00";
    private const string datetimeFriday = "2030-01-04T12:16:22.697099+02:00";
    private const string datetimeSaterday = "2030-01-05T12:16:22.697099+02:00";
    private const string datetimeSunday = "2030-01-06T12:16:22.697099+02:00";
    private const string datetimeMonday = "2030-01-07T12:16:22.697099+02:00";

    [SetUp]
    public void Setup()
    {
        if (dbContext == null)
        {
            dbContext = new BookingContext(
                new DbContextOptionsBuilder<BookingContext>()
                    .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                    .EnableSensitiveDataLogging()
                    .EnableDetailedErrors()
                    .Options
            );

            dbContext.Bookings.Add(new Booking()
            {
                CarId = new Guid("9D2B0228-4D0D-4C23-8B49-01A698857709"),
                StartDate = DateTime.Parse(datetimeSaterday),
                EndDate = DateTime.Parse(datetimeSunday)
            });

            dbContext.SaveChanges();
        }

        _bookingService = new BookingService.Services.BookingService(A.Fake<ILogger<BookingService.Services.BookingService>>(), dbContext);
        _bookingController = new BookingController(A.Fake<ILogger<BookingController>>(), _bookingService);
    }

    [Test]
    public async Task AvailableForRental()
    {
        var result = await _bookingController
            .Get(new Guid("9D2B0228-4D0D-4C23-8B49-01A698857709"), 
                DateTime.Parse(datetimeTuesday), 
                DateTime.Parse(datetimeFriday));
        Assert.That(result.Result.CarId, Is.EqualTo(new Guid("9D2B0228-4D0D-4C23-8B49-01A698857709")));
        Assert.That(result.Result.Available, Is.EqualTo(true));
        Assert.That(result.Success, Is.EqualTo(true));
    }
    
    [Test]
    public async Task EndsOnSameDayAsStart()
    {
        var result = await _bookingController
            .Get(new Guid("9D2B0228-4D0D-4C23-8B49-01A698857709"), 
                DateTime.Parse(datetimeFriday), 
                DateTime.Parse(datetimeSaterday));
        Assert.That(result.Result.CarId, Is.EqualTo(new Guid("9D2B0228-4D0D-4C23-8B49-01A698857709")));
        Assert.That(result.Result.Available, Is.EqualTo(false));
        Assert.That(result.Success, Is.EqualTo(true));
    }
    
    [Test]
    public async Task StartsBeforeReservationAndEndsAfter()
    {
        var result = await _bookingController
            .Get(new Guid("9D2B0228-4D0D-4C23-8B49-01A698857709"), 
                DateTime.Parse(datetimeTuesday), 
                DateTime.Parse(datetimeMonday));
        Assert.That(result.Result.CarId, Is.EqualTo(new Guid("9D2B0228-4D0D-4C23-8B49-01A698857709")));
        Assert.That(result.Result.Available, Is.EqualTo(false));
        Assert.That(result.Success, Is.EqualTo(true));
    }
}