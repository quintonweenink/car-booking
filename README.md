# car-booking
Car booking system

## Assumptions
A Rental price consists of:
* A price per car, per day
* if a rental takes longer than 3 days, the renter gets a discount of 15% over the
  total price.
* Insurance adds 10% per day on top of the car price.
* Snappcar adds 10% per day on top of the car price.
* On Saturday and Sunday the base price of the car goes up with 5%. 


## Starting with the pricing service because you can't book a car without a price

### The way I see it 

The most basic and important initial feature is for the pricing service is for it to return the price of the car based on a start and end date.
Without this we can't have bookings. Lets build and test this first

