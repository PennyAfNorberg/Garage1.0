namespace Garage_1._0
{
    public class Motorcycle : Vehicle
    {
        public int TopSpeed { get; set; }
        public string Brand { get; set; }

        public Motorcycle(string LicensePlateNumber, string Color, int Wheels, int Seats, int TopSpeed, string Brand)
            : base(LicensePlateNumber, Color, Wheels, Seats)
        {
            this.TopSpeed = TopSpeed;
            this.Brand = Brand;
        }
        public Motorcycle()
        {

        }
        public override string ToString()
        {
            return $"Type: { this.Type().Name}\t" + base.ToString() + $"\tTopSpeed: {TopSpeed}\tBrand: {Brand} ";
        }

        public override bool Eq(Vehicle other)
        {
            if (other == null || other.GetType() != this.GetType())
                return false;
            Motorcycle pos = other as Motorcycle;
            return (base.Eq(pos as Vehicle) && this.TopSpeed == pos.TopSpeed && this.Brand == pos.Brand);
            //  return (this.LicensePlateNumber == pos.LicensePlateNumber && this.Color == pos.Color && this.Wheels == pos.Wheels && this.Seats == pos.Seats && this.TopSpeed == pos.TopSpeed && this.Brand == pos.Brand);
        }
    }
}