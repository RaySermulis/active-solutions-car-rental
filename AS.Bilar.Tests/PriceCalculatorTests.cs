using System.ComponentModel;
using AS.Bilar.DAL.Enums;
using AS.Bilar.DAL.Models;
using AS.Bilar.Service;
using FluentAssertions;
using Xunit;

namespace AS.Bilar.Tests
{
    public class PriceCalculatorTests
    {
        [Theory]
        [InlineData(2,1000)]
        [InlineData(3, 1500)]
        [InlineData(4, 2000)]
        [InlineData(0, 500)]
        public void CalculateCarPrice_for_small_cars_calculates_correctly(double days, double expectedPrice)
        {
            //Arrange
            var bookingNumber = "A1";
            var bookingDate = DateTime.Now;
            var baseDailyPrice = 500;

            var startedBooking = CreateSmallCarbooking(BookingStatus.BookingStarted, bookingNumber, bookingDate);
            var finishedBooking = CreateSmallCarbooking(BookingStatus.BookingCompleted, bookingNumber, bookingDate.AddDays(days));
            var completeBookingSet = new List<CarBooking> { startedBooking, finishedBooking };

            //Act
            var result = PriceCalculator.CalculateCarPrice(baseDailyPrice, completeBookingSet);

            //Assert
            result.Should().Be(expectedPrice);
        }

        [Theory]
        [InlineData(2, 1000, 2800)]
        [InlineData(3, 1500, 4200)]
        [Description("Formula for calculating the price: baseDailyPrice * amountOfDays * StandartTariff + baseKmPrice * mileageDriven")]
        public void CalculateCarPrice_for_Kombi_cars_calculates_correctly(double days, int mileage, double expectedPrice)
        {
            //Arrange
            var bookingNumber = "A1";
            var bookingDate = DateTime.Now;
            var baseDailyPrice = 500;
            var standartTariff = 1.3;
            var baseKmPrice = 1.5;
            var kombiTariff = 1.0;

            var startedBooking = CreateKombiCarbooking(BookingStatus.BookingStarted, bookingNumber, bookingDate, 0);
            var finishedBooking = CreateKombiCarbooking(BookingStatus.BookingCompleted, bookingNumber, bookingDate.AddDays(days), mileage);
            var completeBookingSet = new List<CarBooking> { startedBooking, finishedBooking };

            //Act
            var result = PriceCalculator.CalculateCarPrice(baseDailyPrice, baseKmPrice, completeBookingSet, standartTariff, kombiTariff);

            //Assert
            result.Should().Be(expectedPrice);
        }

        [Theory]
        [InlineData(2, 1000, 3550)]
        [InlineData(3, 1500, 5325)]
        [Description("Formula for calculating the price: baseDailyPrice * amountOfDays * StandartTariff + baseKmPrice * mileageDriven * extraTariff")]
        public void CalculateCarPrice_for_Truck_cars_calculates_correctly(double days, int mileage, double expectedPrice)
        {
            //Arrange
            var bookingNumber = "A1";
            var bookingDate = DateTime.Now;
            var baseDailyPrice = 500;
            var standartTariff = 1.3;
            var baseKmPrice = 1.5;
            var truckTariff = 1.5;

            var startedBooking = CreateKombiCarbooking(BookingStatus.BookingStarted, bookingNumber, bookingDate, 0);
            var finishedBooking = CreateKombiCarbooking(BookingStatus.BookingCompleted, bookingNumber, bookingDate.AddDays(days), mileage);
            var completeBookingSet = new List<CarBooking> { startedBooking, finishedBooking };

            //Act
            var result = PriceCalculator.CalculateCarPrice(baseDailyPrice, baseKmPrice, completeBookingSet, standartTariff, truckTariff);

            //Assert
            result.Should().Be(expectedPrice);
        }

        private CarBooking CreateSmallCarbooking(BookingStatus bookingStatus, string bookingNumber, DateTime createDate)
        {
            return new CarBooking
            {
                ID = new Guid(),
                BookingNumber = bookingNumber,
                CarInfo = new Car
                {
                    ID = new Guid(),
                    CarType = CarType.Small,
                    Mileage = 10000
                },
                BookingStatus = bookingStatus,
                CreatedDate = createDate,
                CustomerInfo = new Customer
                {
                    ID = Guid.NewGuid(),
                    PersonalNumber = "19860515-0000"
                }
            };
        }

        private CarBooking CreateKombiCarbooking(BookingStatus bookingStatus, string bookingNumber, DateTime createDate, int mileage)
        {
            return new CarBooking
            {
                ID = new Guid(),
                BookingNumber = bookingNumber,
                CarInfo = new Car
                {
                    ID = new Guid(),
                    CarType = CarType.Kombi,
                    Mileage = mileage
                },
                BookingStatus = bookingStatus,
                CreatedDate = createDate,
                CustomerInfo = new Customer
                {
                    ID = Guid.NewGuid(),
                    PersonalNumber = "19860515-0000"
                }
            };
        }
    }
}