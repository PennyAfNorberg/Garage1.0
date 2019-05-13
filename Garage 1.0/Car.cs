namespace Garage_1._0
{
    public class Car : Vehicle
    {
        public double CylinderVolyme { get; set; }


        public Car(string LicensePlateNumber, string Color, int Wheels, int Seats, double CylinderVolyme)
            : base(LicensePlateNumber, Color, Wheels, Seats)
        {
            this.CylinderVolyme = CylinderVolyme;
        }
        public Car()
        {

        }
        public override string ToString()
        {
            return $"Type: { this.Type().Name}\t" + base.ToString() + $"\tCylinderVolyme: {CylinderVolyme: 0.##}";
        }

        public override bool Eq(Vehicle other)
        {
            if (other == null || other.GetType() != this.GetType())
                return false;
            Car pos = other as Car;
            return (base.Eq(pos as Vehicle) && this.CylinderVolyme == pos.CylinderVolyme);
            // return (this.LicensePlateNumber == pos.LicensePlateNumber && this.Color == pos.Color && this.Wheels == pos.Wheels && this.Seats == pos.Seats && this.CylinderVolyme == pos.CylinderVolyme);
        }
    }
}