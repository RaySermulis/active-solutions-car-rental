using AS.Bilar.DAL.Enums;
using AS.Bilar.DAL.Models;

namespace AS.Bilar.Service
{
    public static class PriceCalculator
    {
        public static double CalculateCarPrice(double baseDailyPrice, List<CarBooking> completeBookings)
        {
            if (!completeBookings.Select(x => x.BookingStatus).Contains(BookingStatus.BookingCompleted))
            {
                //ToDo something to notify/throw etc...
            }

            var bookingDates = completeBookings.Select(x => x.CreatedDate).OrderBy(x => x.TimeOfDay).ToList();
            var amountOfDays = (bookingDates.Max() - bookingDates.Min()).TotalDays;

            //if same day return
            if (amountOfDays == 0)
            {
                amountOfDays = 1;
            }
            return baseDailyPrice * amountOfDays;
        }

        public static double CalculateCarPrice(double baseDailyPrice, double baseKmPrice, List<CarBooking> completeBookings, double standartTariff, double tariff)
        {
            if (!completeBookings.Select(x => x.BookingStatus).Contains(BookingStatus.BookingCompleted))
            {
                //ToDo something to notify/throw etc...
            }

            var bookingDates = completeBookings.Select(x => x.CreatedDate).OrderBy(x => x.TimeOfDay).ToList();
            var amountOfDays = (bookingDates.Max() - bookingDates.Min()).TotalDays;

            var mileageRecords = completeBookings.Select(x => x.CarInfo.Mileage).ToList();
            var mileageDriven = mileageRecords.Max() - mileageRecords.Min();

            return baseDailyPrice * amountOfDays * standartTariff + baseKmPrice * mileageDriven * tariff;
        }
    }
}
