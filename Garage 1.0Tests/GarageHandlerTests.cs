using Microsoft.VisualStudio.TestTools.UnitTesting;
using Garage_1._0;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Garage_1._0.Tests
{
    [TestClass()]
    public class GarageHandlerTests
    {
        // Will chose randomly from a list for this
        private Random Rnd { get; set; } = new Random();

        [TestMethod()]
        [Ignore]
        public void GarageHandlerTest()
        {
            //Arrange

            //act

            //assert
            Assert.Fail();
        }

        [TestMethod()]
        public void InsertmockTest()
        {
            //Arrange
            var capatity1 = Rnd.Next(1, 20);
            var garageHandler1 = new GarageHandler();
            garageHandler1.CreateNewGarage(capatity1);
            //act
            garageHandler1.Insertmock();

            //assert
            Assert.AreEqual(capatity1-2, garageHandler1.Count, $"Count fel");
        }

        [TestMethod()]
        public void GetAllVehiclesTest()
        {
            //Arrange
            var capatity1 = Rnd.Next(1, 20);
            var garageHandler1 = new GarageHandler();
            garageHandler1.CreateNewGarage(capatity1);
            garageHandler1.Insertmock();
            //act
            var res = garageHandler1.GetAllVehicles();

            //assert
            Assert.AreEqual(res.Count(), garageHandler1.Count, $"Count fel");
        }

        [TestMethod()]
        public void ResizeGarageTest()
        {
            //Arrange
            var capatity1 = Rnd.Next(12, 20);
            var capatity2= Rnd.Next(1, 9);
            var garageHandler1 = new GarageHandler();
            garageHandler1.CreateNewGarage(capatity1);
            garageHandler1.Insertmock();
            var wasNoelemets = garageHandler1.Count;
            //act
            var res = garageHandler1.ResizeGarage(capatity2);

            //assert
            Assert.AreEqual(res.Count(), wasNoelemets- capatity2, $"Countfel på bortkastat");
            Assert.AreEqual(capatity2, garageHandler1.Capacity, $"Countfel efter omforming");
            Assert.IsTrue(garageHandler1.Count == garageHandler1.Capacity, $"Är ej ful, skumt");
        }

        [TestMethod()]
        public void GetVehiclesGroupbyTest()
        {
            //Arrange
            var capatity1 =30;
            var garageHandler1 = new GarageHandler();
            garageHandler1.CreateNewGarage(capatity1);
            garageHandler1.Insertmock();

            //act
            var res = garageHandler1.GetVehiclesGroupby();


            //assert
            foreach (var item in res)
            {
                Assert.AreEqual(5,item.Antal, $"Wrong number of {item.Type}");
            }
            


        }

        [TestMethod()]
        public void GetByRegNumberTest()
        {
            //Arrange
            var capatity1 = 22;
            var garageHandler1 = new GarageHandler();
            garageHandler1.CreateNewGarage(capatity1);
            garageHandler1.Insertmock();
            //act
            var res = garageHandler1.GetByRegNumber("DLA 456");

            //assert
            Assert.AreEqual(res.Count(), 1, $"Seems to found wrong elment");

        }

        [TestMethod()]
        public void CreateNewGarageTest()
        {//Arrange
            var capatity1 = Rnd.Next(1, 20);
            var capatity2 = 0;
            var capatity3 = Rnd.Next(-20, -1);
            var garageHandler1 = new GarageHandler();
            var garageHandler2 = new GarageHandler();
            var garageHandler3 = new GarageHandler();

            //act
            garageHandler1.CreateNewGarage(capatity1);
            garageHandler2.CreateNewGarage(capatity2);

            //assert
            Assert.AreEqual(capatity1, garageHandler1.Capacity, $"Count fel");
            Assert.AreEqual(capatity2, garageHandler2.Capacity, $"Count fel");
            Assert.ThrowsException<OverflowException>(() => garageHandler3.CreateNewGarage(capatity3), "Didn't throw overflowExeption with negative capacity");

        }

        [TestMethod()]
        [Ignore]
        public void IsGoodPathTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void GetSubTypeTest()
        {
            //Arrange
            var capatity1 = Rnd.Next(1, 20);
            var garageHandler1 = new GarageHandler();
            garageHandler1.CreateNewGarage(capatity1);

            //act
            var test = garageHandler1.GetSubType();

            //assert
            Assert.AreEqual("Garage_1._0.Vehicle", test.ToString(), $"Fault type");
        }

        [TestMethod()]
        public void AddElementTest()
        {
            //Arrange
            var capatity1 = Rnd.Next(1, 20);
            var capatity2 = 0;

            var garageHandler1 = new GarageHandler();
            var garageHandler2 = new GarageHandler();
            garageHandler1.CreateNewGarage(capatity1);
            garageHandler2.CreateNewGarage(capatity2);
            var test = new Airplane("Airplane2", "White", 3, 45, 23.56);

            //act
            var res1 = garageHandler1.AddElement(test, test.GetType());
            var res2 = garageHandler2.AddElement(test, test.GetType());
            var res3 = garageHandler1.GetByRegNumber("Airplane2").FirstOrDefault();

            //assert
            Assert.IsTrue(res1, $"Seems not have been added in a garage with free space");
            Assert.IsFalse(res2, $"Seems have been added in a garage with no free space");
            Assert.AreEqual(test, res3, $"hmm test isn't in garageHandler1");

        }

        [TestMethod()]
        public void RemoveElementTest()
        {
            //Arrange
            var capatity1 = Rnd.Next(1, 20);
            var garageHandler1 = new GarageHandler();
            var garageHandler2 = new GarageHandler();
            garageHandler1.CreateNewGarage(capatity1);
            garageHandler2.CreateNewGarage(capatity1);
            var test = new Airplane("Airplane2", "White", 3, 45, 23.56);
            garageHandler1.AddElement(test, test.GetType());
            //act
            var res1 = garageHandler1.RemoveElement(test, test.GetType());
            var res2 = garageHandler2.RemoveElement(test, test.GetType());
            var res3 = garageHandler1.GetByRegNumber("Airplane2").FirstOrDefault();

            //assert
            Assert.IsTrue(res1, $"Seems not have been remove in a garage witch it is");
            Assert.IsFalse(res2, $"Seems have been remove in a garage witch it not is");
            Assert.IsNull( res3, $"hmm test be inin garageHandler1, after remove");

        }
    }
}