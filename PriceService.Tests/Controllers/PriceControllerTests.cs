using FakeItEasy;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PriceService.Controllers;
using PriceService.Database;
using PriceService.Services;

namespace PriceService.Tests.Controllers;

public class Tests
{
    private Services.PriceService _priceService;
    private PriceController _priceController;
    private PriceContext dbContext;
    private IBookingService _bookingService;

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
            dbContext = new PriceContext(
                new DbContextOptionsBuilder<PriceContext>()
                    .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                    .EnableSensitiveDataLogging()
                    .EnableDetailedErrors()
                    .Options
            );

            dbContext.Cars.Add(new Car()
            {
                CarId = new Guid("9D2B0228-4D0D-4C23-8B49-01A698857709"),
                Price = 100
            });

            dbContext.SaveChanges();
        }

        _bookingService = A.Fake<IBookingService>();
        _priceService = new Services.PriceService(A.Fake<ILogger<Services.PriceService>>(), dbContext, _bookingService);
        _priceController = new PriceController(A.Fake<ILogger<PriceController>>(), _priceService);
    }
    
    [Test]
    public async Task TestCarDoesNotExist()
    {
        A.CallTo(() => _bookingService.IsCarAvailable(Guid.Empty, DateTime.Now, DateTime.Now))
            .WithAnyArguments()
            .Returns(true);
        var result = await _priceController
            .Get(new Guid("9D2B0228-4D0D-4C23-8B49-01A698837709"), 
                DateTime.Parse(datetimeSaterday), 
                DateTime.Parse(datetimeSunday));
        Assert.That(result.Success, Is.EqualTo(false));
    }
    
    [Test]
    public async Task CarIsAlreadyBooked()
    {
        A.CallTo(() => _bookingService.IsCarAvailable(Guid.Empty, DateTime.Now, DateTime.Now))
            .WithAnyArguments()
            .Returns(true);
        var result = await _priceController
            .Get(new Guid("9D2B0228-4D0D-4C23-8B49-01A698857709"), 
                DateTime.Parse(datetimeSaterday), 
                DateTime.Parse(datetimeSunday));
        Assert.That(result.Success, Is.EqualTo(false));
    }

    [Test]
    public async Task WeekendTwoDayRental()
    {
        A.CallTo(() => _bookingService.IsCarAvailable(Guid.Empty, DateTime.Now, DateTime.Now))
            .WithAnyArguments()
            .Returns(false);
        var result = await _priceController
            .Get(new Guid("9D2B0228-4D0D-4C23-8B49-01A698857709"), 
                DateTime.Parse(datetimeSaterday), 
                DateTime.Parse(datetimeSunday));
        Assert.That(result.Result.CarId, Is.EqualTo(new Guid("9D2B0228-4D0D-4C23-8B49-01A698857709")));
        Assert.That(result.Result.FullPrice, Is.EqualTo(250));
        Assert.That(result.Success, Is.EqualTo(true));
    }
    
    [Test]
    public async Task WeekendOneDayRental()
    {
        A.CallTo(() => _bookingService.IsCarAvailable(Guid.Empty, DateTime.Now, DateTime.Now))
            .WithAnyArguments()
            .Returns(false);
        var result = await _priceController
            .Get(new Guid("9D2B0228-4D0D-4C23-8B49-01A698857709"), 
                DateTime.Parse(datetimeSaterday), 
                DateTime.Parse(datetimeSaterday));
        Assert.That(result.Result.CarId, Is.EqualTo(new Guid("9D2B0228-4D0D-4C23-8B49-01A698857709")));
        Assert.That(result.Result.FullPrice, Is.EqualTo(125));
        Assert.That(result.Success, Is.EqualTo(true));
    }
    
    [Test]
    public async Task WeekdayDayRental()
    {
        A.CallTo(() => _bookingService.IsCarAvailable(Guid.Empty, DateTime.Now, DateTime.Now))
            .WithAnyArguments()
            .Returns(false);
        var result = await _priceController
            .Get(new Guid("9D2B0228-4D0D-4C23-8B49-01A698857709"), 
                DateTime.Parse(datetimeFriday), 
                DateTime.Parse(datetimeFriday));
        Assert.That(result.Result.CarId, Is.EqualTo(new Guid("9D2B0228-4D0D-4C23-8B49-01A698857709")));
        Assert.That(result.Result.FullPrice, Is.EqualTo(120));
        Assert.That(result.Success, Is.EqualTo(true));
    }
    
    [Test]
    public async Task WeekdayDayFourDayRental()
    {
        A.CallTo(() => _bookingService.IsCarAvailable(Guid.Empty, DateTime.Now, DateTime.Now))
            .WithAnyArguments()
            .Returns(false);
        var result = await _priceController
            .Get(new Guid("9D2B0228-4D0D-4C23-8B49-01A698857709"), 
                DateTime.Parse(datetimeTuesday), 
                DateTime.Parse(datetimeFriday));
        Assert.That(result.Result.CarId, Is.EqualTo(new Guid("9D2B0228-4D0D-4C23-8B49-01A698857709")));
        Assert.That(result.Result.FullPrice, Is.EqualTo(408));
        Assert.That(result.Success, Is.EqualTo(true));
    }
}