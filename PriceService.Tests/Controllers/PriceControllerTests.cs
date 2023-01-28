using CarBooking.Controllers;
using Microsoft.Extensions.Logging;
using PricingService.Database;
using FakeItEasy;
using Microsoft.EntityFrameworkCore;

namespace PriceService.Tests;

public class Tests
{
    private PriceController _priceController;
    private PriceContext dbContext;

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
        }

        var logger = A.Fake<ILogger<PriceController>>();

        _priceController = new PriceController(logger, dbContext);
    }
    
    [Test]
    public async Task TestCarDoesNotExist()
    {
        var result = await _priceController
            .Get(new Guid("9D2B0228-4D0D-4C23-8B49-01A698837709"), 
                DateTime.Parse(datetimeSaterday), 
                DateTime.Parse(datetimeSunday));
        Assert.That(result.Success, Is.EqualTo(false));
    }

    [Test]
    public async Task WeekendTwoDayRental()
    {
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
        var result = await _priceController
            .Get(new Guid("9D2B0228-4D0D-4C23-8B49-01A698857709"), 
                DateTime.Parse(datetimeTuesday), 
                DateTime.Parse(datetimeFriday));
        Assert.That(result.Result.CarId, Is.EqualTo(new Guid("9D2B0228-4D0D-4C23-8B49-01A698857709")));
        Assert.That(result.Result.FullPrice, Is.EqualTo(408));
        Assert.That(result.Success, Is.EqualTo(true));
    }
}