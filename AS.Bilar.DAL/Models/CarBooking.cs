using AS.Bilar.DAL.Enums;

namespace AS.Bilar.DAL.Models
{
    public class CarBooking
    {
        public Guid ID { get; set; }
        public string BookingNumber { get; set; }
        public Car CarInfo { get; set; }
        public Customer CustomerInfo { get; set; }
        public DateTime CreatedDate { get; set; }
        public BookingStatus BookingStatus { get; set; }
    }
}
