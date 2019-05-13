

namespace Garage_1._0
{
    public abstract class  Vehicle 
    {
        public string LicensePlateNumber { get; set; }
        public string Color { get; set; }
        public int Wheels { get; set; }
        public int Seats { get; set; }


        public Vehicle()
        {

        }
        public Vehicle(string LicensePlateNumber, string Color, int Wheels, int Seats)
        {
            this.LicensePlateNumber = LicensePlateNumber;
            this.Color = Color;
            this.Wheels = Wheels;
            this.Seats = Seats;
        }

        public override string ToString()
        {
            return $"LicensePlateNumber: {LicensePlateNumber}\tColor: {Color}\tWheels: {Wheels}\tSeats: {Seats} ";
        }

        public virtual bool Eq(Vehicle other)
        {
            return (this.LicensePlateNumber == other.LicensePlateNumber && this.Color == other.Color && this.Wheels == other.Wheels && this.Seats == other.Seats);
        }
    }
}