
using AS.Bilar.DAL.Enums;

namespace AS.Bilar.DAL.Models
{
    public class Car
    {
        public Guid ID { get; set; }
        public string RegNumber { get; set; }
        public int Mileage { get; set; }
        public CarType CarType { get; set; }
    }
}
