using AS.Bilar.DAL.Models;

namespace AS.Bilar.DAL.Repositories
{
    public interface ICarBookingRepository
    {
        Task<CarBooking> GetCarBooking(string bookingId);
        Task<List<CarBooking>> GetCarBookings(string bookingId);
        Task InsertCarBooking(CarBooking carBooking);
    }

    public class CarBookingRepository : ICarBookingRepository
    {
        public Task<CarBooking> GetCarBooking(string bookingId)
        {
            throw new NotImplementedException();
        }

        public Task<List<CarBooking>> GetCarBookings(string bookingId)
        {
            throw new NotImplementedException();
        }

        public Task InsertCarBooking(CarBooking carBooking)
        {
            throw new NotImplementedException();
        }
    }
}
