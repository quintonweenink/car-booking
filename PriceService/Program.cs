using CarBooking;
using PricingService.Database;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddDbContext<PriceContext>();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<PriceService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// app.UseHttpsRedirection();

using (var scope = app.Services.CreateScope())
{
    var service = scope.ServiceProvider;
    var context = service.GetService<PriceContext>();
    
    context.Cars.AddRange(new Car
        {
            CarId = new Guid("9D2B0228-4D0D-4C23-8B49-01A698857709"),
            Price = 500
        },
        new Car
        {
            CarId = new Guid("9D2B0228-4D0D-4C23-8B49-01A698857708"),
            Price = 400
        });
    context.SaveChanges();
}


app.UseAuthorization();

app.MapControllers();

app.Run();