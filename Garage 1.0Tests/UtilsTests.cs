using Microsoft.VisualStudio.TestTools.UnitTesting;
using Garage_1._0;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Garage_1._0.Utils;

namespace Garage_1._0.Tests
{
    [TestClass()]
    public class UtilsTests
    {
        [TestMethod()]
        [Ignore]
        public void TypeTest()
        { // a bit abstract
            Assert.Fail();
        }

        [TestMethod()]
        public void IsOnlyOfClassTest()
        {
            //arrange
            var list1 = new List<Vehicle>
               {
                 { new Airplane("Airplane1", "White", 3, 235, 40) },
                 { new Boat("Boaty macBoatFace", "Blue", 0, 10, 2) },
                 { new Car("Abc 123", "Metallic", 4, 5, 2) },
                 { new Bus("XYZ 987", "Red", 4, 45, DateTime.Now.AddMonths(2)) }
            };
            var list2 = new List<Vehicle>
               {
                 { new Airplane("Airplane1", "White", 3, 235, 40) },
                 { new Airplane("Airplane2", "Black", 3, 35, 27)  }
            };
            var acttype = new Airplane("Airplane1", "White", 3, 235, 40).GetType();
            //act
            var res1 = Utils.IsOnlyOfClass(list1, acttype);
            var res2 = Utils.IsOnlyOfClass(list2, acttype);

            //assert
            Assert.IsFalse(res1, $"Error repported just airplanes when mixed");
            Assert.IsTrue(res2, $"Error reported not just aiplanes when just airplanes");
        }

        [TestMethod()]
        public void ConvertToClassTest()
        {
            //arrange
            var list1 = new List<Vehicle>
               {
                 { new Airplane("Airplane1", "White", 3, 235, 40) },
                 { new Boat("Boaty macBoatFace", "Blue", 0, 10, 2) },
                 { new Car("Abc 123", "Metallic", 4, 5, 2) },
                 { new Bus("XYZ 987", "Red", 4, 45, DateTime.Now.AddMonths(2)) }
            };
            var list2 = new List<Vehicle>
               {
                 { new Airplane("Airplane1", "White", 3, 235, 40) },
                 { new Airplane("Airplane2", "Black", 3, 35, 27)  }
            };
            var acttype = new Airplane("Airplane1", "White", 3, 235, 40).GetType();
            //act
            var res1 = Utils.ConvertToClass(list1, acttype);
            var res2 = Utils.ConvertToClass(list2, acttype).ToList();

            //assert
            Assert.IsNull(res1, "A mixed list shall return null");
            CollectionAssert.AllItemsAreInstancesOfType(res2, acttype, "A non midexlist shall be converted at run time");
        }

        [TestMethod()]
        [Ignore]
        public void GetAllTest()
        { // How? uses console
            Assert.Fail();
        }

        [TestMethod()]
        [Ignore]
        public void GetKeyTypesTest()
        {
            // How? uses console
            Assert.Fail();
        }

        [TestMethod()]
        [Ignore]
        public void DynamicCastTest()
        {
            // is tested by  IsOnlyOfClassTest()
            Assert.Fail();
        }

        [TestMethod()]
        public void GetAllChildrenTest()
        {
            //arrange
            var list1 = new List<Vehicle>();
            var actSupertype= list1.GetType().GetGenericArguments().Single();
            var airtype = new Airplane("Airplane1", "White", 3, 235, 40).GetType();
            var boatType = new Boat("Boaty macBoatFace", "Blue", 0, 10, 2).GetType();
            var busType = new Bus("XYZ 987", "Red", 4, 45, DateTime.Now.AddMonths(2)).GetType();
            var carType = new Car("Abc 123", "Metallic", 4, 5, 2).GetType();
            var motorcycleType = new Motorcycle("QWE 456", "Green", 2, 2, 250, "Kwasaki").GetType();
            //act
            var children = Utils.GetAllChildren(actSupertype);
            
            //assert
            Assert.AreEqual(5, children.Count(), "Count error for children to Vehicle");

            foreach (var item in children)
            {
                if (item != airtype && item != boatType && item != busType && item != carType && item != motorcycleType)
                    Assert.Fail($"unexperted type {item.ToString()}");
            }


        }

        [TestMethod()]
        public void GetFirstCtorsParametesTest()
        {
            //arrange
            var airtype = new Airplane("Airplane1", "White", 3, 235, 40).GetType();

            //act 
            var para = Utils.GetFirstCtorsParametes(airtype);

            // public Airplane(string LicensePlateNumber, string Color, int Wheels, int Seats, double WingSpan)

            //assert
            Assert.AreEqual(5, para.Count(), "Count error for airplanes first ctor");

            for (int i = 0; i < para.Count(); i++)
            {
                if (para[i].Position != i)
                    Assert.Fail($"Wrong position for {para[i].Name}");
                if (i==0)
                {
                    if(para[i].Name != "LicensePlateNumber")
                        Assert.Fail($"Wrong Name for {para[i].Name}");
                }
                if (i==0 || i==1)
                {
                    if(para[i].ParameterType.Name.ToString() != "String")
                        Assert.Fail($"Wrong type for {para[i].Name}");
                }
                if (i == 1)
                {
                    if (para[i].Name != "Color")
                        Assert.Fail($"Wrong Name for {para[i].Name}");
                }
                if (i == 2)
                {
                    if (para[i].Name != "Wheels")
                        Assert.Fail($"Wrong Name for {para[i].Name}");
                }
                if (i == 2 || i == 3)
                {
                    if (para[i].ParameterType.Name.ToString() != "Int32")
                        Assert.Fail($"Wrong type for {para[i].Name}");
                }
                if (i == 3)
                {
                    if (para[i].Name != "Seats")
                        Assert.Fail($"Wrong Name for {para[i].Name}");
                }
                if (i == 4)
                {
                    if (para[i].Name != "WingSpan")
                        Assert.Fail($"Wrong Name for {para[i].Name}");
                    if (para[i].ParameterType.Name.ToString() != "Double")
                        Assert.Fail($"Wrong type for {para[i].Name}");
                }
            }

        }

        [TestMethod()]
        public void PrettyPrintTest()
        {

            //arrange
            char enter = '\r';
            char backspace = '\b';
            char space = ' ';
            char escape = '\u001b';
            char key = 'J';

            //act
            var enterres = Utils.PrettyPrint(enter);
            var backspaceres= Utils.PrettyPrint(backspace);
            var spaceres = Utils.PrettyPrint(space);
            var escaperes = Utils.PrettyPrint(escape);
            var keyres = Utils.PrettyPrint(key);

            //assert
            Assert.AreEqual("Enter", enterres, $"Enter was wrongly printed as {enterres}");
            Assert.AreEqual("Backspace", backspaceres, $"Backspace was wrongly printed as {backspaceres}");
            Assert.AreEqual("Space", spaceres, $"Space was wrongly printed as {spaceres}");
            Assert.AreEqual("Escape", escaperes, $"Escape was wrongly printed as {escaperes}");
            Assert.AreEqual("J", keyres, $"J was wrongly printed as {keyres}");



        }
    }
}