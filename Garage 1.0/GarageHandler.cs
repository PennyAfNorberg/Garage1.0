using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
namespace Garage_1._0
{
    public class GarageHandler
    {
      /// <summary>
      /// Declares and init Garage
      /// Querys Garage
      /// </summary>

        private  Garage<Vehicle> garage;
        public int Capacity { get => garage.Capacity; }
        public int Count { get => garage.Count; }

        public bool IsFull { get => garage.IsFull; }

        public GarageHandler()
        {
            /*
            garage = new Garage<Vehicle>(5);
            // några test
            var airplane = new Airplane("plane MacPlaneface", "green", 3, 234, 23.56);
            garage.Add(airplane);
            var boat = new Boat("Boat macboatFace", "White", 0, 5000, 100);
            garage.Add(boat);
            var check = GetAllVehicles();


            var res = GetVehiclesGroupby();
            foreach (var item in res)
            {
                Console.WriteLine($"{item.Type}\t{item.Antal}");
            };
            */
        }

        private Random Rnd { get; set; } = new Random();


        public bool Insertmock()
        {
            var lista = new List<Vehicle>
            {
                { new Airplane("Airplane1", "White", 3, 235, 40) },
                { new Boat("Boaty macBoatFace", "Blue", 0, 10, 2) },
                { new Car("Abc 123", "Metallic", 4, 5, 2) },
                { new Bus("XYZ 987", "Red", 4, 45, DateTime.Now.AddMonths(2)) },
                { new Motorcycle("QWE 456", "Green", 2, 2, 250,"Kwasaki") },
                { new Airplane("Airplane2", "Black", 3, 35, 27)  },
                { new Boat("Brother of Boaty macBoatFace", "Magenta", 0, 100, 20) },
                { new Car("QPC 167", "Yellow", 4, 9, 1.6) },
                { new Bus("APQ 134", "Blue", 4, 46, DateTime.Now.AddMonths(3)) },
                { new Motorcycle("DLA 456", "Red", 2, 2, 275,"HD") },
                { new Airplane("Airplane11", "Pink", 3, 35, 15)  },
                { new Boat("Boaty macBoatFace3", "White", 0, 15, 6) },
                { new Car("PAD 164", "Brown", 4, 5, 2) },
                { new Bus("XGR 561", "Cyan", 4, 40, DateTime.Now.AddMonths(1)) },
                { new Motorcycle("QWE 466", "Black", 2, 2, 250,"Nokia") },
                { new Airplane("Airplane21", "Yellow", 3, 335, 45)  },
                { new Boat("Boaty macBoatFace2", "Brown", 0, 150, 64.3f) },
                { new Car("AKE 534", "Cyan", 4, 2, 4.5) },
                { new Bus("ALR 561", "Pink", 4, 47, DateTime.Now.AddMonths(5)) },
                { new Motorcycle("WSA 456", "Blue", 2, 3, 150,"Suziki") },
                { new Airplane("Airplane5", "Green", 3, 330, 34.56)  },
                { new Boat("Boaty macBoatFace4", "Cyan", 0, 155, 74.3f) },
                { new Car("ALE 861", "Violett", 4, 5, 1.8) },
                { new Bus("ELS 561", "Pink", 4, 47, DateTime.Now.AddMonths(5)) },
                { new Motorcycle("WSA 466", "Blue", 2, 3, 150,"HD") },

            };
            int i = 2;
            int listlengthpre = lista.Count;
            while (lista.Count>0 && i < Math.Min(garage.Capacity, listlengthpre + 2))
            {
                var item = lista[Rnd.Next(0, lista.Count() - 1)];
                if (!(AddElement(item)))
                {
                    lista.Remove(item);
                    continue;
                }
                lista.Remove(item);
                i++;

            }
            return true;
        }



        public Vehicle[] GetAllVehicles()
        {
            return garage.Where(Vehicle=>Vehicle!=null).ToArray();
           
           // return garage.GetAllVehicles();
        }

        public Vehicle[] ResizeGarage(int capacity)
        {
            Vehicle[] leftovers = garage.Resize(capacity );
            return leftovers;
        }

        public List<groupbytype> GetVehiclesGroupby()
        {
            var res = GetAllVehicles()
                .Where(v => (v!=null))
              //  .GroupBy(v => v.Type().Name)
               .GroupBy(v => v.GetType().Name)
                .Select(g => new groupbytype
                {
                    Type = g.Key,
                    Antal = g.Count()
                }).ToList<groupbytype>()
                ;

            return res;
          
        }



        public Vehicle[] GetByRegNumber(string LicensePlateNumber)
        {
            var res = GetAllVehicles()
                .Where(v => (v != null))
                .Where(v => v.LicensePlateNumber == LicensePlateNumber)
                .ToArray<Vehicle>()
                ;
            return res;
        }

        public void CreateNewGarage(int capacity)
        {
            garage = new Garage<Vehicle>(capacity);
            // mock some data

        }

        private bool Save(string path)
        {
            var FileIO = new FileIO();
            return FileIO.Save(path, this.garage.GetAllVehicles());

        }

        private void Load(string path)
        {
            var FileIO = new FileIO();
            try
            {
               // this.garage = garage.Load(path, this.garage);
                this.garage.LoadData(
                    FileIO.Load(path));
            }
            catch (Exception)
            {

                throw;
            }
            

        }

        internal void ProcessPath(string path, ConsoleMenu.FileTypes activeFileType)
        {
            
            if (activeFileType == ConsoleMenu.FileTypes.Load)
            {
                Load(path);
                // garage.Load(path);
                /*  var document = XDocument.Load("Test.xml");
                  var res = document.Element("Vehicles")?.Elements("Vehicle") ?? Enumerable.Empty<XElement>();
                  foreach (var item in res)
                  {
                      var work = item.Elements().InDocumentOrder();
                      int numerofpars = work.Count() - 1;
                      var typename = work.FirstOrDefault();
                      if (numerofpars > -1)
                      {
                          object[] ParamArray = new Object[numerofpars];
                          for (int i = 0; i < ParamArray.Length; i++)
                          {

                          }

                      }

                  }*/


            }
            else if(activeFileType == ConsoleMenu.FileTypes.Save)
            {
                Save(path);
                //garage.Save(path);
                /* var document = new XDocument();

                 document.Save(path);
                 */
            }

           
        }

        public bool IsUniq(Type actType, string LicensePlateNumber)
        {
            if (actType.Name == "Car" || actType.Name == "Bus" || actType.Name == "Motorcycle")
            {
                foreach (var item in garage.Where(v => v != null))
                {
                    if (item.GetType().Name == "Car" || item.GetType().Name == "Bus" || item.GetType().Name == "Motorcycle")
                    {
                        if (item.LicensePlateNumber.ToLower() == LicensePlateNumber.ToLower())
                            return false;
                    }
                }
                return true;
            }
            else
            {
                foreach (var item in garage.Where(v => v != null))
                {
                    if (item.GetType() == actType)
                    {
                        if (item.LicensePlateNumber.ToLower() == LicensePlateNumber.ToLower())
                            return false;
                    }
                }
                return true;
            }
        }

        private bool IsUniq(Vehicle vehicle)
        {
            var actype = vehicle.GetType();
            if (actype.Name == "Car" || actype.Name == "Bus" || actype.Name == "Motorcycle")
            {
                foreach (var item in garage.Where(v => v!= null))
                {
                    if (item.GetType().Name == "Car" || item.GetType().Name == "Bus" || item.GetType().Name == "Motorcycle")
                    {
                        if (item.LicensePlateNumber.ToLower() == vehicle.LicensePlateNumber.ToLower())
                            return false;
                    }
                }
                return true;
            }
            else
            {
                foreach (var item in garage.Where(v => v != null))
                {
                    if(item.GetType()== actype)
                    {
                        if (item.LicensePlateNumber.ToLower() == vehicle.LicensePlateNumber.ToLower())
                            return false;
                    }
                }
                return true;    
            }
        }

        public bool IsGoodPath(string path, ConsoleMenu.FileTypes activeFileType)
        {
            if(activeFileType == ConsoleMenu.FileTypes.Load)
            {
                try
                {
                    FileStream fp = new FileStream(path, FileMode.Open);
                    fp.Close();
                }
                catch (FileNotFoundException)
                {

                    return false;
                }
                catch (IOException)
                {

                    return false;
                }
                catch (Exception)
                {
                    throw;
                }
                return true;
            }
            else if (activeFileType == ConsoleMenu.FileTypes.Save)
            {
                try
                {
                    FileStream fp = new FileStream(path, FileMode.OpenOrCreate);
                    fp.Close();
                }
                catch (IOException)
                {

                    return false;
                }
                catch (Exception)
                {
                    throw;
                }
                return true;
            }
            else
            {
                throw new ArgumentOutOfRangeException();
            }
        }

        public Type GetSubType()
        {
            return garage.GetType().GetGenericArguments().Single();
        }

            private  bool AddElement(Vehicle element)
            { // felsök här
              /*check:
               *  Om element är null
               *  Om acttype ärver garage.GetType().GetGenericArguments().Single() in?
               *  att cast funkar, men det ska den pga acttype
               */
              /*if ( element.GetType() != garage.GetType().GetGenericArguments().Single())
           throw new ArgumentException("Wrong type in AddElement");
         */


                if (element == null)
                    throw new ArgumentNullException("Null Element in AddElement");

                try
                { // fast fail?
                    if (IsUniq(element))
                        return garage.Add(element);
                    return false;
                }
                catch (Exception)
                {

                    throw new ArgumentException("Wrong type in AddElement");
                }
            }


        public bool AddElement(object element, Type actType)
        { // felsök här
          /*check:
           *  Om element är null
           *  Om acttype ärver garage.GetType().GetGenericArguments().Single() in?
           *  att cast funkar, men det ska den pga acttype
           */
          /*if ( element.GetType() != garage.GetType().GetGenericArguments().Single())
       throw new ArgumentException("Wrong type in AddElement");
     */


            if (element == null)
                throw new ArgumentNullException("Null Element in AddElement");

            try
            {
                var realelem = Utils.DynamicCast(element, actType);
                return garage.Add(realelem);
            
            }
            catch (Exception)
            {

                throw new ArgumentException("Wrong type in AddElement");
            }
        }


        public bool RemoveElement(object element, Type actType)
        {

            /* if (element == null || element.GetType() != garage.GetType().GetGenericArguments().Single())
                 throw new ArgumentException("Wrong type in RemoveElement");
              */

            if (element == null)
                throw new ArgumentNullException("Null Element in RemoveElement");

            try
            {
                var realelem = Utils.DynamicCast(element, actType);
                return garage.Remove(realelem);
            }
            catch (Exception)
            {

                throw new ArgumentException("Wrong type in RemoveElement");
            }
           // return garage.Remove(Utils.DynamicCast(element, actType));
        }
    }

     
}