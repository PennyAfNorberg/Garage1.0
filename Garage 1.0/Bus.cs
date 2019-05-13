using System;

namespace Garage_1._0
{
    public class Bus : Vehicle
    {
        public DateTime NextService { get; set; }

        public Bus(string LicensePlateNumber, string Color, int Wheels, int Seats, DateTime NextService)
            : base(LicensePlateNumber, Color, Wheels, Seats)
        {
            this.NextService = NextService;
        }
        public Bus()
        {

        }
        public override string ToString()
        {
            return $"Type: { this.Type().Name}\t" + base.ToString() + $"\tNextService: {NextService: yy-MM-dd}";
        }

        public override bool Eq(Vehicle other)
        {
            if (other == null || other.GetType() != this.GetType())
                return false;
            Bus pos = other as Bus;
            return (base.Eq(pos as Vehicle) && this.NextService == pos.NextService);
            // return (this.LicensePlateNumber == pos.LicensePlateNumber && this.Color == pos.Color && this.Wheels == pos.Wheels && this.Seats == pos.Seats && this.NextService == pos.NextService);
        }
    }
}