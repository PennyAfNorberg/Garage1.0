using System;
using System.Collections.Generic;
using System.Linq;


namespace Garage_1._0
{
    public static class  Utils
    {
        

        public static System.Type Type<T>(this T v) => typeof(T);

        /// <summary>
        /// Checks if a IEnumerable<object> only contains objects of class toClass at runtime
        /// </summary>
        /// <param name="List"> To check</param>
        /// <param name="ToClass">Check for</param>
        /// <returns></returns>
        public static bool IsOnlyOfClass(this IEnumerable<object> List, Type ToClass)
        {
            bool answer = false;

            foreach (var item in List)
            {
                try
                {
                    DynamicCast(item, ToClass);
                }
                catch (Exception)
                {

                    return false;
                }
                answer = true;
            }
            return answer;
        }

        /// <summary>
        /// Convertso to class if possible at run time
        /// </summary>
        /// <param name="List"></param>
        /// <param name="ToClass"></param>
        /// <returns></returns>
        public static IEnumerable<dynamic> ConvertToClass(this IEnumerable<dynamic> List, Type ToClass)
        {
            if(IsOnlyOfClass(List,ToClass))
            {
                List<dynamic> answer = new List<dynamic>(); 
                foreach (var item in List)
                {
                    answer.Add(DynamicCast(item, ToClass));
                }
                return answer;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Ask right getthing
        /// </summary>
        /// <param name="type">needed</param>
        /// <returns></returns>
        public static dynamic GetAll(string type)
        {
            /*   switch (type)
               {
                   case "System.Int32":
                       return GetInt();


                   case "System.String":
                       return GetString();

                   case "System.Single":
                       return GetFloat();

                   case "System.Double":
                       return GetDouble();

                   case "System.DateTime":
                       return GetDateTime();

                   default:
                       return null;

               }
               */
            switch (type)
            {
                case "System.Int32":
                    return getIndata("A number is needed {0} isn't a number", "The number {0] isn't valid", Int32.Parse, (v => true));
                case "System.String":
                    // return getIndata<String>("", "Bad indata, please try again", (v => v), String.IsNullOrEmpty);
                    return GetString();

                case "System.Single":
                    return getIndata("A number is needed {0} isn't a number, decimals are ok", "The number {0] isn't valid", System.Single.Parse, (v => true));

                case "System.Double":
                    return getIndata("A number is needed {0} isn't a number, decimals are ok", "The number {0] isn't valid", System.Double.Parse, (v => true));

                case "System.DateTime":
                    return getIndata("A Datetime is needed {0} isn't a datetime", "The number {0] isn't valid", System.DateTime.Parse, (v => true));

                default:
                    return null;
            }
        }

        private static T getIndata<T>(string errPromtConvert, string errPromtValidtate, Func<string,T> convert, Func<T, bool> validate) where T : new()
        {

            // promt

            // convert // checka 
            // validate
            var input = Console.ReadLine();
            T converted= new T();
            bool check = false;
            while (!check)
            {
                try
                {
                    converted = convert(input);
                }
                catch (Exception)
                {
                    Console.WriteLine(errPromtConvert, input);
                    input = Console.ReadLine();
                    continue;
                }
                if(validate(converted))
                {
                    check = true;
                }
                else
                {
                    Console.WriteLine(errPromtValidtate, input);
                    input = Console.ReadLine();
                    continue;
                }
            }

            return converted;
        }
/*
        private static int GetInt()
        {
            
            var input = Console.ReadLine();
            int capacity = -1;
            while (!(Int32.TryParse(input, out capacity)))
            {
                Console.WriteLine($"A number is needed {input} isn't a number");
                input = Console.ReadLine();
            }
            Console.WriteLine("");
            return capacity;
        }

        private static DateTime GetDateTime()
        {

            var input = Console.ReadLine();
            DateTime capacity = new DateTime(); ;
            while (!(DateTime.TryParse(input, out capacity)))
            {
                Console.WriteLine($"A datetime is needed {input} isn't a datetime");
                input = Console.ReadLine();
            }
            Console.WriteLine("");
            return capacity;
        }

        private static float GetFloat()
        {
            
            var input = Console.ReadLine();
            float capacity = -1;
            while (!(Single.TryParse(input, out capacity)))
            {
                Console.WriteLine($"A numbers is needed {input} isn't a number, floating number is ok");
                input = Console.ReadLine();
            }
            Console.WriteLine("");
            return capacity;
        }

        private static double GetDouble()
        {
            
            var input = Console.ReadLine();
            double capacity = -1;
            while (!(Double.TryParse(input, out capacity)))
            {
                Console.WriteLine($"A numbers is needed {input} isn't a number, floating number is ok");
                input = Console.ReadLine();
            }
            Console.WriteLine("");
            return capacity;
        }
        */

        private static string GetString()
        {
            
            string input = Console.ReadLine();
            while (string.IsNullOrEmpty(input))
            {
                Console.WriteLine("Bad indata, please try again");
                input = Console.ReadLine();
            }
            Console.WriteLine("");
            return input;
        }

        /// <summary>
        /// Ask for key in VehicleKeys, returns value.
        /// </summary>
        /// <param name="VehicleKeys"></param>
        /// <returns></returns>
        public static T GetKeyTypes<T>(SortedDictionary<char, T> VehicleKeys)
        {
            
            var choosedKey = Char.ToUpper(Console.ReadKey().KeyChar);
            T actVehicle;
            while (!(VehicleKeys.TryGetValue(choosedKey, out actVehicle)))
            {
                Console.WriteLine($"{PrettyPrint(choosedKey)} isn't on the meny, please rechoose");
                choosedKey = char.ToUpper(Console.ReadKey().KeyChar);
            }
            Console.WriteLine("");
            return actVehicle;
        }

        /// <summary>
        /// Cast Dynamic
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="castTo"></param>
        /// <returns></returns>
        public static dynamic DynamicCast(dynamic obj, Type castTo)
        {
            try
            {
                var svar = Convert.ChangeType(obj, castTo);
                return svar;
            }
            catch (Exception)
            {

                throw;
            }
        }

        /// <summary>
        /// Get all children to a superclass
        /// </summary>
        /// <param name="SuperClass"></param>
        /// <returns></returns>
        public static IEnumerable<Type> GetAllChildren(Type SuperClass)
        {
            return SuperClass
                .Assembly.GetTypes()
                .Where(t => t.IsSubclassOf(SuperClass) && !t.IsAbstract)
                .Select(t => t);
        }

        /// <summary>
        /// Gets all first ctors to a type, for add, and search
        /// </summary>
        /// <param name="actType"></param>
        /// <returns></returns>

        public static System.Reflection.ParameterInfo[] GetFirstCtorsParametes(Type actType)
        {
            var ctors = actType.GetConstructors();
            // Class we just checks first ctor, if more than one a wider strukture is needed
            return ctors[0].GetParameters();
        }

        /// <summary>
        /// Print some chars pretty
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string PrettyPrint(char key)
        {
            switch (key)
            {
                case '\r':
                    return "Enter";
                case '\b':
                    return "Backspace";
                case ' ':
                    return "Space";
                case '\u001b':
                    return "Escape";

                default:
                    return key.ToString();
                 
            }
        }
    } 
}
