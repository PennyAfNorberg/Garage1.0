namespace Garage_1._0
{
    public class Boat : Vehicle
    {

        public float Tonnage { get; set; }


        public Boat(string LicensePlateNumber, string Color, int Wheels, int Seats, float Tonnage)
            : base(LicensePlateNumber, Color, Wheels, Seats)
        {
            this.Tonnage = Tonnage;
        }
        public Boat()
        {

        }
        public override string ToString()
        {
            return $"Type: { this.Type().Name}\t" + base.ToString() + $"\tTonnage: {Tonnage: 0.##}";
        }

        public override bool Eq(Vehicle other)
        {
            if (other == null || other.GetType() != this.GetType())
                return false;
            Boat pos = other as Boat;
            return (base.Eq(pos as Vehicle) && this.Tonnage == pos.Tonnage);
           // return (this.LicensePlateNumber == pos.LicensePlateNumber && this.Color == pos.Color && this.Wheels == pos.Wheels && this.Seats == pos.Seats && this.Tonnage == pos.Tonnage);
        }
    }
}