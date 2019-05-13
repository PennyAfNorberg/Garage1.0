using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Garage_1._0
{ //,
    public class Garage<T>: IEnumerable<T> where T : Vehicle​
    {
        private T[] theGarage;
        private int nextindex; // Points to the next index to write, also handelse full by setting -1

        public bool IsFull { get => (nextindex < 0); }

        public int Capacity { get; private set; }

        public int Count { get  => theGarage.Where(v=>v!=null).Count();  }

        public Garage()
        {

        }
        public Garage(int Capacity)
        {
            try
            {
                theGarage = new T[Capacity];
            }
            catch (OutOfMemoryException)
            {

                throw new OutOfMemoryException($"out of mem");

            }
            catch(Exception)
            {
                throw;

            }
            
            this.Capacity = Capacity;
            nextindex = (Capacity > 0) ? 0 : -1;

        }
        
        
        public T[] GetAllVehicles()
        {
            return theGarage.ToArray<T>();
        }
        
        

        public bool Remove(T variable)
        { // returna de man tog bort?
            int wheretodelete = -1;
            for (int i = 0; i < theGarage.Length; i++)
            {
                if (theGarage[i] != null)
                {
                    if (theGarage[i].Eq(variable))
                    {
                        theGarage[i] = null;
                        if (wheretodelete == -1)
                        {
                            nextindex = i;
                            wheretodelete++;
                        }
                    }
                }
            }
            return (wheretodelete == -1) ? false : true;
            
        }

        public T[] Resize(int capacity)
        {
            var temp = theGarage.ToArray<T>();
            List<T> svar = new List<T>();
          //  theGarage.CopyTo(theGarage, 5);
            theGarage = new T[capacity];
            this.Capacity = capacity;
            this.nextindex = (capacity > 0) ? 0 : -1;
            int i = 0;
            while(nextindex > -1 && i< temp.Length)
            {
                Add(temp[i]);
                i++;
            }
            while(i < temp.Length)
            {
                if(temp[i]!=null)
                    svar.Add(temp[i]);
                i++;
            }
            return svar.ToArray<T>();
        }

        public bool Add(T variable)
        {

            if (nextindex > -1)
            {
                theGarage[nextindex] = variable;
                nextindex = -1; //full
                for (int i = 0; i < theGarage.Length; i++)
                {
                    if (theGarage[i] == null)
                    {
                        nextindex = i;
                        break;
                    }
                }
                return true;
            }
            return false;

        }



        /*
        public bool Save(string path, Garage<T> theGarage)
        { //flytta upp eller till egna klasser och nyttja using för fp
            using (FileStream fp = new FileStream(path, FileMode.OpenOrCreate))
            {
                DataContractJsonSerializer ser = new DataContractJsonSerializer(typeof(Garage<T>), GetKnownTypes());
                ser.WriteObject(fp, theGarage);
            }
            //fp.Close();
            return true;
        }
        
        public Garage<Vehicle> Load(string path,Garage<Vehicle> theGarage)
        { //flytta upp eller till egna klasser och nyttja using för fp
            using (FileStream fp = new FileStream(path, FileMode.Open))
            {
                DataContractJsonSerializer ser = new DataContractJsonSerializer(typeof(Garage<Vehicle>), GetKnownTypes());
                fp.Position = 0;
                theGarage = (Garage<Vehicle>)ser.ReadObject(fp);
            }
           // this.theGarage= theGarage
            //fp.Close();
            return theGarage;
        }

        private static IEnumerable<Type> _knowTypes;

        private static IEnumerable<Type> GetKnownTypes()
        {// hmm borde väl vara dynamisk denna med då?
            if (_knowTypes == null)
                _knowTypes = Assembly.GetExecutingAssembly()
                                        .GetTypes()
                                        .Where(t => typeof(Airplane).IsAssignableFrom(t)
                                        || typeof(Boat).IsAssignableFrom(t)
                                        || typeof(Bus).IsAssignableFrom(t)
                                        || typeof(Car).IsAssignableFrom(t)
                                        || typeof(Motorcycle).IsAssignableFrom(t)
                                        )               
                                        .ToList();
            return _knowTypes;
        }
        */

        public IEnumerator<T> GetEnumerator()
        {
            for (int i = 0; i < theGarage.Length; i++)
            {
                if(theGarage[i]!=null)
                {
                    yield return theGarage[i];
                }
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return (IEnumerator)this.GetEnumerator();
        }

        internal void LoadData(T[] vehicle)
        {
            this.theGarage = vehicle;
            this.Capacity = vehicle.Length;
            this.nextindex = -1;
            for (int i = 0; i < theGarage.Length; i++)
            {
                if (theGarage[i] == null)
                {
                    this.nextindex = i;
                    break;
                }
            }
        }
    }
}