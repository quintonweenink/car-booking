using BookingService.Database;
using BookingService.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddDbContext<BookingContext>();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<IBookingService, BookingService.Services.BookingService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

var datetimeTuesday = "2030-01-01T12:16:22.697099+02:00";
var datetimeFriday = "2030-01-04T12:16:22.697099+02:00";
var datetimeSaterday = "2030-01-05T12:16:22.697099+02:00";
var datetimeSunday = "2030-01-06T12:16:22.697099+02:00";
var datetimeMonday = "2030-01-07T12:16:22.697099+02:00";

using (var scope = app.Services.CreateScope())
{
    var service = scope.ServiceProvider;
    var context = service.GetService<BookingContext>();
    
    context.Bookings.AddRange(new Booking
        {
            CarId = new Guid("9D2B0228-4D0D-4C23-8B49-01A698857709"),
            StartDate = DateTime.Parse(datetimeTuesday),
            EndDate = DateTime.Parse(datetimeFriday)
        },
        new Booking
        {
            CarId = new Guid("9D2B0228-4D0D-4C23-8B49-01A698857708"),
            StartDate = DateTime.Parse(datetimeSunday),
            EndDate = DateTime.Parse(datetimeMonday)
        });
    context.SaveChanges();
}

// app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();