using Microsoft.VisualStudio.TestTools.UnitTesting;
using Garage_1._0;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Threading.Tasks;

namespace Garage_1._0.Tests
{
    // testDb Logic
    // All tables have schema: tests and prefix Garage
    // 
    // Might need result and if it shall it and if exeption

    [TestClass()]
    public class GarageTests
    {

        // Viktigt med rätt variabelnamn här nedand
        public TestContext TestContext { get; set; }

        // need some Mocks
        // Will chose randomly from a list for this
        private Random Rnd { get; set; } = new Random();

        private List<Vehicle> mockDataSource = new List<Vehicle>
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
                { new Airplane("Airplane1", "Pink", 3, 35, 15)  },
                { new Boat("Boaty macBoatFace", "White", 0, 15, 6) },
                { new Car("PAD 164", "Brown", 4, 5, 2) },
                { new Bus("XGR 561", "Cyan", 4, 40, DateTime.Now.AddMonths(1)) },
                { new Motorcycle("QWE 466", "Black", 2, 2, 250,"Nokia") },
                { new Airplane("Airplane2", "Yellow", 3, 335, 45)  },
                { new Boat("Boaty macBoatFace2", "Brown", 0, 150, 64.3f) },
                { new Car("AKE 534", "Cyan", 4, 2, 4.5) },
                { new Bus("ALR 561", "Pink", 4, 47, DateTime.Now.AddMonths(5)) },
                { new Motorcycle("WSA 456", "Blue", 2, 3, 150,"Suziki") },
                { new Airplane("Airplane5", "Green", 3, 330, 34.56)  },
                { new Boat("Boaty macBoatFace", "Cyan", 0, 155, 74.3f) },
                { new Car("ALE 861", "Violett", 4, 5, 1.8) },
                { new Bus("ELS 561", "Pink", 4, 47, DateTime.Now.AddMonths(5)) },
                { new Motorcycle("WSA 456", "Blue", 2, 3, 150,"HD") },


            };

        private string Printdatarow()
        {
            var strwork = new StringBuilder();
            var table = TestContext.DataRow.Table;
            // foreach(DataRow row in table.Rows)
            // {
            foreach (DataColumn column in table.Columns)
            {
                strwork.Append($"{column}: {TestContext.DataRow[column]}, ");
            }


            //}

            return strwork.ToString();
        }
        private int GetInt(string v)
        {
            if (Int32.TryParse(TestContext.DataRow[v].ToString(), out int result))
            {
                return result;
            }
            return 0;
        }

        private float Getfloat(string v)
        {
            if (Single.TryParse(TestContext.DataRow[v].ToString(), out float result))
            {
                return result;
            }
            return 0;
        }



        /// <summary>
        /// Test cases  non negative capatity =>  the right capatity
        /// Capatity =0 => se above
        /// Capatity <0  => throws error
        /// </summary>
        [TestMethod()]
        public void GarageTest()
        {
            //Arrange
            var capatity1 = Rnd.Next(1, 20);
            var capatity2 = 0;
            var capatity3 = Rnd.Next(-20, -1);

            //act
            var garage1 = new Garage<Vehicle>(capatity1);
            var garage2 = new Garage<Vehicle>(capatity2);
            Garage<Vehicle> garage3;
            //assert

            Assert.AreEqual(capatity1, garage1.Capacity, $"Count fel");
            Assert.AreEqual(capatity2, garage2.Capacity, $"Count fel");
            Assert.ThrowsException<OverflowException>(() => garage3 = new Garage<Vehicle>(capatity3),"Didn't throw overflowExeption with negative capacity");
        }

        /// <summary>
        /// Test cases: Add ok, check if ok
        ///             Add when full => no exeption, just a error
        ///  Test cases after this one act for add is good and will use mockdata
        /// </summary>

        [TestMethod()]
        public void AddTest()
        {
            //Arrange
            var capatity1 = 1;
            var capatity2 = 0;
            var exepted1 = mockDataSource[Rnd.Next(0, mockDataSource.Count - 1)];
            var exepted2 = mockDataSource[Rnd.Next(0, mockDataSource.Count - 1)];
            while(exepted1.Eq(exepted2))
            {
                 exepted2 = mockDataSource[Rnd.Next(0, mockDataSource.Count - 1)];
            }
            var garage1 = new Garage<Vehicle>(capatity1);
            var garage2 = new Garage<Vehicle>(capatity2);

            //act


            var result1 = garage1.Add(exepted1);
            var result2 = garage2.Add(exepted2);

            Vehicle existsin1 = garage1.FirstOrDefault(v => v == exepted1);
            Vehicle existsin2 = garage2.FirstOrDefault(v => v == exepted2);

            //assert
            Assert.IsTrue(result1, "Add Ok reported failure");
            Assert.IsFalse(result2, "Add failed reported ok");
            Assert.IsNotNull(existsin1, "Add ok element isn't in container");
            Assert.IsNull(existsin2, "Add failed element is in container");

        }
        /// <summary>
        /// Test cases remove when full => not full
        /// Remove => not in
        /// Remove fail if not in.
        /// </summary>

        [TestMethod()]
        public void RemoveTest()
        {
            //Arrange
            var capatity1 = 2;
            var garage1 = new Garage<Vehicle>(capatity1);
            var item1 = mockDataSource[Rnd.Next(0, mockDataSource.Count - 1)];
            var item2 = mockDataSource[Rnd.Next(0, mockDataSource.Count - 1)];
            var item3= mockDataSource[Rnd.Next(0, mockDataSource.Count - 1)];
            while (item1.Eq(item2))
            {
                item2 = mockDataSource[Rnd.Next(0, mockDataSource.Count - 1)];
            }
            while (item1.Eq(item3) || item2.Eq(item3))
            {
                item3 = mockDataSource[Rnd.Next(0, mockDataSource.Count - 1)];
            }
            garage1.Add(item1);
            garage1.Add(item2);
            var wasfull = garage1.IsFull;

            //act
            var result4 = garage1.Remove(item2);
            Vehicle existsin1 = garage1.FirstOrDefault(v => v == item2);
            var result5= garage1.Remove(item3);

            //assert
            Assert.IsTrue(wasfull & (!garage1.IsFull), "When removing an element from a full garage the garage didn't get non-full");
            Assert.IsNull(existsin1, "When removing an element the element wasn't removed");
            Assert.IsFalse(result5, "when removings a non existing element the remove method retruern true");

        }

        /// <summary>
        /// Test Cases
        ///     Resize a fulllist to something bigger => new list isn't full 
        ///         and no elements are returned
        ///     Resize a list with n elements to a list with m ellements m less than n, get (n-m) elements returned 
        ///         who was in the list pre but not post.
        /// </summary>
        [TestMethod()]
        public void ResizeTest()
        {
            //Arrange
            var capatity1 = 2;
            var newcapatity1 = 4;
            var capatity2 = 6;
            var newcapatity2 = 4;
            var garage1 = new Garage<Vehicle>(capatity1);
            var garage2 = new Garage<Vehicle>(capatity2);
            var garage3 = new Garage<Vehicle>(capatity2);
            var item1 = mockDataSource[Rnd.Next(0, mockDataSource.Count - 1)];
            for (int i = 0; i < capatity1; i++)
            {
                while(garage1.Where(v =>v!=null).Any(v => v.Eq(item1)))
                {
                    item1 = mockDataSource[Rnd.Next(0, mockDataSource.Count - 1)];
                }
                garage1.Add(item1);
            }

            for (int i = 0; i < capatity2; i++)
            {
                while (garage2.Where(v => v != null).Any(v => v.Eq(item1)))
                {
                    item1 = mockDataSource[Rnd.Next(0, mockDataSource.Count - 1)];
                }
                garage2.Add(item1);
                garage3.Add(item1);
            }

            //act
            var wasfull = garage1.IsFull;
            var leftovers1 = garage1.Resize(newcapatity1);
            var leftovers2 = garage2.Resize(newcapatity2);
            var isfull = garage1.IsFull;
            var expectedcount1 = 0;
            var actualcount1 = leftovers1.Length;
            var expectedcount2 = (capatity2-newcapatity2) ;
            var actualcount2 = leftovers2.Length;
            var checkIfNoLeftovers2InGarage2 = true;
            // does Linq support own predicats in stead of eqauls?
            foreach (var itemIn in garage2.Where(v => v != null))
            {
                foreach (var itemOut in leftovers2)
                {
                    if(itemIn.Eq(itemOut))
                    {
                        checkIfNoLeftovers2InGarage2 = false;
                        break;
                    }
                }
            }
            var checkifLeftooversWasInGarage = true;
            foreach (var itemOut in leftovers2) 
            {
                checkifLeftooversWasInGarage= checkifLeftooversWasInGarage  | garage3.Any(v => v.Eq(itemOut));
                if (checkifLeftooversWasInGarage == false)
                    break;
            }


            //assert
            Assert.IsTrue(wasfull & (!isfull), "When rezing a full garage to something bigger the garage stayed full");
            Assert.AreEqual(expectedcount1, actualcount1, "When rezing a full garage to something bigger Some items was returned, and shouldn't");
            Assert.AreEqual(expectedcount2, actualcount2, "When resizing to as smaller, and some pre element didn't fit, the returned elemetns was the wrong count");
            Assert.IsTrue(checkIfNoLeftovers2InGarage2, "When resizing to as smaller, and some pre element didn't fit, Some returned elments stays in garage, and shouldn't");
            Assert.IsTrue(checkifLeftooversWasInGarage, "When resizing to as smaller, and some pre element didn't fit, Some returned elemts wasn't in the garage");
        }



        [TestMethod()]
        [Ignore]
        public void SaveTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        [Ignore]
        public void LoadTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        [Ignore]
        public void GetEnumeratorTest()
        {
            Assert.Fail();
        }
    }
}