using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization.Json;

namespace Garage_1._0
{
    class FileIO
    {
                public bool Save(string path, Vehicle[] theGarage)
        { //flytta upp eller till egna klasser och nyttja using för fp
            using (FileStream fp = new FileStream(path, FileMode.OpenOrCreate))
            {
                DataContractJsonSerializer ser = new DataContractJsonSerializer(typeof(Vehicle[]), GetKnownTypes());
                ser.WriteObject(fp, theGarage);
            }
            //fp.Close();
            return true;
        }
        
        public Vehicle[] Load(string path )
        { //flytta upp eller till egna klasser och nyttja using för fp
            Vehicle[] theGarage;
            using (FileStream fp = new FileStream(path, FileMode.Open))
            {
                DataContractJsonSerializer ser = new DataContractJsonSerializer(typeof(Vehicle[]), GetKnownTypes());
                fp.Position = 0;
                theGarage = (Vehicle[])ser.ReadObject(fp);
            }
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
                                        || typeof(Vehicle).IsAssignableFrom(t)
                                        || typeof(Garage<Vehicle>).IsAssignableFrom(t)
                                        )               
                                        .ToList();
            return _knowTypes;
        }

    }
}
