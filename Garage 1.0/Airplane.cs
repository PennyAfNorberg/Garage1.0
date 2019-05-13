namespace Garage_1._0
{
    public class Airplane : Vehicle
    {
        public double WingSpan { get; set; }


        public Airplane(string LicensePlateNumber, string Color, int Wheels, int Seats, double WingSpan)
            : base(LicensePlateNumber, Color, Wheels,Seats)
        {
            this.WingSpan = WingSpan;
        }

        public Airplane()
        {

        }
        public override string ToString()
        {
            return $"Type: { this.Type().Name}\t"+base.ToString() + $"\tWingSpan: {WingSpan}";
        }


        public override bool Eq(Vehicle other)
        {
            if (other == null || other.GetType() != this.GetType())
                return false;
            Airplane pos = other as Airplane;
            return (base.Eq(pos as Vehicle) && this.WingSpan == pos.WingSpan);
            // return (this.LicensePlateNumber == pos.LicensePlateNumber && this.Color == pos.Color && this.Wheels == pos.Wheels && this.Seats == pos.Seats && this.WingSpan == pos.WingSpan);
        }

    }
}