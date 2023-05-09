using AS.Bilar.DAL.Enums;
using AS.Bilar.DAL.Models;
using AS.Bilar.DAL.Repositories;


namespace AS.Bilar.Service
{
    public interface ICarBookingService
    {
        Task RegisterCarBooking(string bookingNumber, Car carInfo, Customer customerDetails);
        Task FinishCarBooking(string bookingNumber);
    }

    public class CarBookingService : ICarBookingService
    {
        private readonly ICarBookingRepository _carBookingRepository;

        private const double BaseDailyPrice = 500;
        private const int BaseKmPrice = 20;

        private const double KombiStandartTariff = 1.3;
        private const double KombiTariff = 1;

        private const double TruckTariff = 1.5;
        private const double TruckStandartTariff = 1.5;


        public CarBookingService(ICarBookingRepository carBookingRepository)
        {
            _carBookingRepository = carBookingRepository;
        }

        public async Task RegisterCarBooking(string bookingNumber, Car carInfo, Customer customerDetails)
        {
            var carBookingRecord = new CarBooking
            {
                ID = new Guid(),
                BookingNumber = bookingNumber,
                CarInfo = carInfo,
                CustomerInfo = customerDetails,
                CreatedDate = DateTime.Now,
                BookingStatus = BookingStatus.BookingStarted
            };

            await _carBookingRepository.InsertCarBooking(carBookingRecord);
        }

        public async Task FinishCarBooking(string bookingNumber)
        {
            //Register a finished car rental booking
            var existingBooking = await _carBookingRepository.GetCarBooking(bookingNumber);
            if (existingBooking is not null)
            {
                var finishedCarBooking = new CarBooking
                {
                    ID = new Guid(),
                    BookingNumber = bookingNumber,
                    CarInfo = existingBooking.CarInfo,
                    CustomerInfo = existingBooking.CustomerInfo,
                    CreatedDate = DateTime.Now,
                    BookingStatus = BookingStatus.BookingCompleted
                };

                await _carBookingRepository.InsertCarBooking(finishedCarBooking);
            }
            else
            {
                //ToDo something to notify/throw etc...
                return;
            }

            //Calculate the price for the finished booking
            var completeBookingSet = await _carBookingRepository.GetCarBookings(bookingNumber);
            switch (existingBooking.CarInfo.CarType)
            {
                case CarType.Small:
                    {
                        PriceCalculator.CalculateCarPrice(BaseDailyPrice, completeBookingSet);
                        break;
                    }
                case CarType.Kombi:
                    {
                        PriceCalculator.CalculateCarPrice(BaseDailyPrice, BaseKmPrice, completeBookingSet, KombiStandartTariff, KombiTariff);
                        break;
                    }
                case CarType.Truck:
                    {
                        PriceCalculator.CalculateCarPrice(BaseDailyPrice, BaseKmPrice, completeBookingSet, TruckStandartTariff, TruckTariff);
                        break;
                    }
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}

