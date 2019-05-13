using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using static Garage_1._0.Utils;
using System.Reflection;
using System.Globalization;

namespace Garage_1._0
{
    /// <summary>
    ///  Meny
    ///  Some filtering
    /// </summary>


    public class ConsoleMenu
    {
        #region props and fields

        private GarageHandler garageHandler = new GarageHandler(); // glob
        private enum Submenus
        { Main, Second, DefineVehicle, GetSearchParameters } // glob

        internal enum SearchTypes
        { Search, Remove } //DefineVehicle
        public enum FileTypes
        { Load, Save } //FileDialog
        private readonly SortedDictionary<Submenus, SortedDictionary<char, Action<string>>> supeMenu; // glob

        /*
        private class ContainerAction
        {
            Action<string> Action;
            string Desc;
        }

    // alt  SortedDictionary<char,class(Action<string>, string)>
    private readonly SortedDictionary<char, ContainerAction> Check;
     */


        private readonly SortedDictionary<char, Action<string>> mainLoopSwitch;
        private readonly SortedDictionary<char, Action<string>> secondLoopSwitch;
        private readonly SortedDictionary<char, Action<string>> defineVehicleSwitch;
        private readonly SortedDictionary<char, string> defineVehicleDesc;
        private readonly SortedDictionary<char, Action<string>> getSearchParametersSwitch;
        private readonly SortedDictionary<char, string> getSearchParametersDesc;


        private readonly SortedDictionary<char, Type> vehicleKeys = new SortedDictionary<char, Type>();

        private Submenus activeMenu;

        private readonly Dictionary<Type, System.Reflection.ParameterInfo[]> savedConstructors = new Dictionary<Type, System.Reflection.ParameterInfo[]>();

        private SearchTypes activeSearchTypes;
        private FileTypes activeFileType;
        private List<Parameter> parameters = new List<Parameter>();
        private Type actType;
        private Type subType;
        internal IEnumerable<Type> GarageChildren;
        private bool inLoop = true;
        // for search
        private string searchPredicateString = "(v =>";
        private IEnumerable<dynamic> searchRes;
        private int startOnExpr = 0;
        private dynamic gottenValue;
        private dynamic gottenValue2;

        private readonly Dictionary<string, Type> paramtoParamtype;
        //IEnumerable<Vehicle>
        //  private IEnumerable<Type> GarageChildren;
        private IEnumerable<Type> parameterChildren;
        private int thisOrdinal;
        readonly MethodInfo startsWithMethod = typeof(string).GetMethod("StartsWith", new[] { typeof(string), typeof(bool),typeof(CultureInfo) });
        // gives String.startsWith(string search, bool ignoreCase,CultureInfo) 

        #endregion

        #region constructor
        public ConsoleMenu()
        {
            activeMenu = Submenus.Main; // glob
            actType = null; //DefineVehicle
            mainLoopSwitch = new SortedDictionary<char, Action<string>>
            {
                {'1', BuildGarage },
                {'L', LoadGarage },
                 {'Q',  Quit }
            };
            

            // string bla=mainLoopSwitch[char.Q].Method.Name;

            secondLoopSwitch = new SortedDictionary<char, Action<string>>
            {
                {'1', ListAllVehicles },
                {'2', ListPerType },
                {'3', AddVehicle },
                {'4', RemoveVehicle },
                {'5', SearchByLicenseNumber },
                {'6', ResizeGarage },
                {'7', SearchVehicle },
                {'M', LoadMock },
                {'S', SaveGarage },
                
                {'Q',  Quit }
            };


            defineVehicleSwitch = new SortedDictionary<char, Action<string>>
           {
                {'1', VehicleType },
                //{char.D2, LicensePlateNumber },
               // {char.D3, Color },
               // {char.D4, Wheels },
               // {char.D5, Seats },
               // {char.D6, SearchVehicle },
               // {char.D7, SaveGarage },
               // {char.Enter, Done},
               {char.ToUpper('\b'), GoUp },
                {'Q',  Quit }
            }; // add some more when D1 is set, redraw options, add save when all parametsers are set.
               // show choosed options in menu
               //  DefineVehicleDesc.OrderBy((k, v) => k);
            defineVehicleDesc = new SortedDictionary<char, string>
            {
                {'1', "VehicleType" },
                {char.ToUpper('\b'), "GoUp" },
                {'Q',  "Quit" }
            };

            getSearchParametersSwitch = new SortedDictionary<char, Action<string>>
            {
                {'A', SearchForVehicleType  },
                {'B', SearchForLicensePlateNumber },
                {'C', SearchForColor },
                {'D', SearchForWheels },
                {'E', SearchForSeats },
            /*    {char.A,  SearchAddAnAnd },
                {char.O,  SearchAddAnOr },
                {char.N,  SearchAddANot},
                {char.W,  SearchAddALeftParatheses },
                {char.E,  SearchAddARightParatheses },*/
                {char.ToUpper('\r'), Done},
                {char.ToUpper('\b'), GoUp },
                {'Q',  Quit }
            };
            getSearchParametersDesc = new SortedDictionary<char, string>
            {
                {'A', "Search for VehicleType"},
                {'B', "Search for LicensePlateNumber" },
                {'C', "Search for Color" },
                {'D', "Search for Wheels" },
                {'E', "Search for Seats" },
                {char.ToUpper('\r'), "Done"},
                {char.ToUpper('\b'), "Go Up" },
                {'Q',  "Quit" }
            };

            supeMenu = new SortedDictionary<Submenus, SortedDictionary<char, Action<string>>>
            {
                {Submenus.Main, mainLoopSwitch },
                {Submenus.Second, secondLoopSwitch },
                {Submenus.DefineVehicle, defineVehicleSwitch}, // spec 2 dir + done
                {Submenus.GetSearchParameters, getSearchParametersSwitch }

            };
            int a=0;
            string b = "";
            float c = 0;
            double d = 0;

            var IntType = a.GetType();
            var StringType = b.GetType();
            var FloatType = c.GetType();
            var DoubleType = d.GetType();

            paramtoParamtype = new Dictionary<String, Type>
            {
                { "System.Int32", (new IntParameter()).GetType() },
                { "System.String", (new StringParameter()).GetType() },
                { "System.Single" , (new FloatParameter()).GetType() },
                { "System.Double", (new  DoubleParameter()).GetType() },
                { "System.DateTime", (new  DateTimeParameter()).GetType() }
            };




            /*
             * 
             * public void PrintParameters()
    {
    var ctors = typeof(A).GetConstructors();
    // assuming class A has only one constructor
    var ctor = ctors[0];
    foreach (var param in ctor.GetParameters())
    {
        Console.WriteLine(string.Format(
            "Param {0} is named {1} and is of type {2}",
            param.Position, param.Name, param.ParameterType));
    }
    }

    */
        }


        #endregion

        #region Files
        /// <summary>
        ///  Asks path from user and then do load/save 
        ///  Doen't affect menus
        /// </summary>

        private void ProcessPath()
        {
            var path = GetAll("System.String");
            
            while(!(garageHandler.IsGoodPath(path, activeFileType)))
            {
                Console.WriteLine($"unable to open {path}");
                path = GetAll("System.String");
            }
            garageHandler.ProcessPath(path, activeFileType);

        }
        #endregion

        #region search

        /*
        private void SearchAddANot(string obj)
        {
            searchPredicateString += " !";
        }

        private void SearchAddARightParatheses(string v)
        {
            searchPredicateString += ") ";
        }

        private void SearchAddALeftParatheses(string v)
        {
            searchPredicateString += " (";
        }

        private void SearchAddAnOr(string v)
        {
            searchPredicateString += " || ";
        }

        private void SearchAddAnAnd(string v)
        {
            searchPredicateString += " && ";
        }
        */
        /// <summary>
        /// Ask user how to search the specicif attributes per subclass, is we are down 1 subclass
        /// int: ==
        /// string : startwith
        /// float: between a and b
        /// double: between a and b
        /// DateTime: between a and b
        /// </summary>
        /// <param name="v">Name_Ordinal_Type for the attribut to ask for, from </param>
        private void SearchForGeneral(string v)
        {
     
            // v: Name_Ordinal_Type
            var vparts = v.Split('_');
            thisOrdinal = Int32.Parse(vparts[1]);
            Console.WriteLine($"Please state the {vparts[0]} to search");
            Console.WriteLine("");
            gottenValue = GetAll(vparts[2]);
            
            switch (vparts[2])
            {
                case "System.Int32":
                    {
                        if (startOnExpr++ == 0)
                        {
                            searchPredicateString += $" v.{vparts[0]} == \"{ gottenValue }\"";
                        }
                        else
                        {
                            searchPredicateString += $" and v.{vparts[0]} == \"{ gottenValue }\"";
                        }
                        searchRes = searchRes
                         .Where(SubSearchforGeneral)
    
                        ;
                    }
                    break;
                case "System.String":
                    {
                        if (startOnExpr++ == 0)
                        {
                            searchPredicateString += $" v.{vparts[0]} like \"{ gottenValue }*\"";
                        }
                        else
                        {
                            searchPredicateString += $" and v.{vparts[0]} like \"{ gottenValue }*\"";
                        }
                        searchRes = searchRes
                         .Where(SubSearchforGeneral);
                    }
                    break;
                case "System.DateTime":
                case "System.Double":
                case "System.Singel":
                    {
                        Console.Write(" and ");
                        gottenValue2 = GetAll(vparts[2]);
                        if (startOnExpr++ == 0)
                        {
                            searchPredicateString += $" v.{vparts[0]} between \"{ gottenValue }\" and \"{gottenValue2}\"";
                        }
                        else
                        {
                            searchPredicateString += $" and v.{vparts[0]} between \"{ gottenValue }\" and \"{gottenValue2}\"";
                        }
                        searchRes = searchRes
                         .Where(SubSearchforGeneralbetween)

                        ;
                    }
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// Search For VehicleType
        ///  do affect menu
        /// </summary>
        /// <param name="nostring"></param>

        private void SearchForVehicleType(string nostring)
        {
            Console.WriteLine("Choose vehicle type");
            Console.WriteLine("");
            foreach (var item in vehicleKeys)
            {
                Console.WriteLine($"Press {item.Key.ToString()} \t {item.Value.Name}");
            }
            var choosedtypeType = GetKeyTypes(vehicleKeys);
            var choosedtype = choosedtypeType.Name;
            if (startOnExpr++ == 0)
            {
                searchPredicateString += " v.GetType().Name == \"" + choosedtype + "\"";
            }
            else
            {
                searchPredicateString += " and  v.GetType().Name == \"" + choosedtype + "\"";
            }
            searchRes = searchRes
                .Where(p => p.GetType().Name == choosedtype) 
                ;
            CheckAndSetClass();
        }




        /// <summary>
        /// Search For LicensePlateNumber
        /// do affect menu
        /// </summary>
        /// <param name="v"></param>
        private void SearchForLicensePlateNumber(string v)
        {
            Console.WriteLine("Please state the licenseplatenumber to search");
            Console.WriteLine("");
            var LicensePlateNumber = GetAll("System.String");
            if (startOnExpr++ == 0)
            {
                searchPredicateString += " v.LicensePlateNumber == \"" + LicensePlateNumber + "*\"";
            }
            else
            {
                searchPredicateString += " and v.LicensePlateNumber == \"" + LicensePlateNumber + "*\"";
            }
            searchRes = searchRes
                .Where(p => p.LicensePlateNumber.StartsWith(LicensePlateNumber))
                ;

            CheckAndSetClass();
        }

        /// <summary>
        /// Search For Color
        /// do affect menu
        /// </summary>
        /// <param name="v"></param>
        private void SearchForColor(string v)
        {
            Console.WriteLine("Please state the Color to search");
            var Color = GetAll("System.String");
            if (startOnExpr++ == 0)
            {
                searchPredicateString += " v.Color == \"" + Color + "*\"";
            }
            else
            {
                searchPredicateString += " and v.Color == \"" + Color + "\"";
            }
            searchRes = searchRes
                .Where(p => p.Color.StartsWith(Color))
                ;
            CheckAndSetClass();

        }

        /// <summary>
        /// Search For Wheels
        /// do affect menu
        /// </summary>
        /// <param name="v"></param>
        private void SearchForWheels(string v)
        {
            Console.WriteLine("Please state the number of wheels to search");
            var Wheels = GetAll("System.Int32");
            if (startOnExpr++ == 0)
            {
                searchPredicateString += " v.Wheels == " + Wheels;
            }
            else
            {
                searchPredicateString += " and v.Wheels == " + Wheels;
            }
            searchRes = searchRes
                .Where(p => p.Wheels == Wheels)
                ;
            CheckAndSetClass();
        }

        /// <summary>
        /// Search For Seats
        /// do affect menu
        /// </summary>
        /// <param name="v"></param>

        private void SearchForSeats(string v)
        {
            Console.WriteLine("Please state the number of Seats to search");
            var Seats = GetAll("System.Int32");
            if (startOnExpr++ == 0)
            {
                searchPredicateString += " v.Seats == " + Seats;
            }
            else
            {
                searchPredicateString += " and v.Seats == " + Seats;
            }
            searchRes = searchRes
                .Where(p => p.Seats == Seats)
                ;
            CheckAndSetClass();

        }
        /// <summary>
        /// Checks if searchRes only contains one class and 
        /// if is convert to that class and take away the VehicleType question
        /// saves the type in actType 
        /// and add more questions
        /// do affect menu
        /// </summary>
        private void CheckAndSetClass()
        {
            foreach (var ToClass in GarageChildren)
            {
                if (searchRes.IsOnlyOfClass(ToClass))
                {
                    searchRes = searchRes.ConvertToClass(ToClass); //.Cast<Vehicle>()  //<--- obra
                  //  ;
                    getSearchParametersSwitch.Remove('A');
                    getSearchParametersDesc.Remove('A');
                    actType = ToClass;
                    AddNewSearchOptions();
                }
            }
        }

        /// <summary>
        /// Add the class specific questions
        /// do affect menu
        /// </summary>

        private void AddNewSearchOptions()
        {
            FillConstructordirectory();
            int i = Convert.ToInt32('F');
            char keychar;
            for (int j = 4; j < savedConstructors[actType].Length; j++)
            {
                keychar = (char)i;
                getSearchParametersSwitch.Add(keychar, SearchForGeneral);
                getSearchParametersDesc.Add(keychar, $"Search for {savedConstructors[actType][j].Name}");
                i++;
            }

        }

        /// <summary>
        /// Make the SearchOptions new again
        /// do affect menu
        /// </summary>
        private void EmptySearchOptions()
        {
            getSearchParametersSwitch.Clear();
            getSearchParametersDesc.Clear();

            getSearchParametersSwitch.Add('A', SearchForVehicleType);
            getSearchParametersSwitch.Add('B', SearchForLicensePlateNumber);
            getSearchParametersSwitch.Add('C', SearchForColor);
            getSearchParametersSwitch.Add('D', SearchForWheels);
            getSearchParametersSwitch.Add('E', SearchForSeats);
            getSearchParametersSwitch.Add(char.ToUpper('\r'), Done);
            getSearchParametersSwitch.Add(char.ToUpper('\b'), GoUp);
            getSearchParametersSwitch.Add('Q', Quit);

            getSearchParametersDesc.Add('A', "Search for VehicleType");
            getSearchParametersDesc.Add('B', "Search for LicensePlateNumber");
            getSearchParametersDesc.Add('C', "Search for Color");
            getSearchParametersDesc.Add('D', "Search for Wheels");
            getSearchParametersDesc.Add('E', "Search for Seats");
            getSearchParametersDesc.Add(char.ToUpper('\r'), "Done");
            getSearchParametersDesc.Add(char.ToUpper('\b'), "Go Up");
            getSearchParametersDesc.Add('Q', "Quit");
        }

        /// <summary>
        /// Make a Expression ask then InvokeLambda
        /// == and startswith
        /// doesn't affect menu
        /// </summary>
        /// <param name="v">to quest</param>
        /// <returns></returns>
        private bool SubSearchforGeneral(Object v)
        {
            var thisparas = savedConstructors[actType];
            var ParaName = thisparas.Where(para => para.Position == thisOrdinal).Select(para => para.Name).FirstOrDefault();
            var ParaType = thisparas.Where(para => para.Position == thisOrdinal).Select(para => para.ParameterType).FirstOrDefault();
            var parameter = Expression.Parameter(v.GetType(), "x");
            var member = Expression.Property(parameter, ParaName); //x.Id
            var constant = Expression.Constant(gottenValue);
            var constant2 = Expression.Constant(true);
            var constant3= Expression.Constant(CultureInfo.CurrentCulture);
            Expression[] constantlist = new Expression[]
            {
                constant,
                constant2,
                constant3
            };
            dynamic body = null;
            try
            {
                switch (ParaType.ToString())
                {
                    case "System.DateTime":
                    case "System.Double":
                    case "System.Singel":
                    case "System.Int32":
                        {

                            body = Expression.Equal(member, constant);

                        }
                        break;
                    case "System.String":
                        {
                            body = Expression.Call( member, startsWithMethod, constantlist);
                        }
                        break;


                    default:
                        return false;

                }

            }
            catch (Exception)
            {

                return false;
            }
            return InvokeLambda(v, parameter, body);
        }


        /// <summary>
        /// Make a Expression ask then InvokeLambda
        /// between
        /// doesn't affect menu
        /// </summary>
        /// <param name="v"></param>
        /// <returns></returns>

        private bool SubSearchforGeneralbetween(Object v)
        {
            var thisparas = savedConstructors[actType];
            var ParaName = thisparas.Where(para => para.Position == thisOrdinal).Select(para => para.Name).FirstOrDefault();
            var ParaType = thisparas.Where(para => para.Position == thisOrdinal).Select(para => para.ParameterType).FirstOrDefault();
            var parameter = Expression.Parameter(v.GetType(), "x");
            var member = Expression.Property(parameter, ParaName); //x.Id
            var constant1 = Expression.Constant(gottenValue);
            var constant2 = Expression.Constant(gottenValue2);
            dynamic body;
            try
            {
                var nV = DynamicCast(v, actType);
                switch (ParaType.ToString())
                {
                    case "System.DateTime":
                    case "System.Double":
                    case "System.Singel":
                        {

                            body = Expression.AndAlso(Expression.GreaterThanOrEqual(member, constant1), Expression.LessThanOrEqual(member, constant2));

                            /* if (nV.ParaName >= gottenValue && nV.ParaName <= gottenValue2)
                                 return true;
                             else
                                 return false;

                              */
                        }
                        break;


                    default:
                        return false;

                }
            }
            catch (Exception)
            {

                throw;
            }
            return InvokeLambda(v, parameter, body);
        }

        /// <summary>
        ///  Executes Lambda
        ///  doesn't affect menu
        /// </summary>
        /// <param name="v">searchres</param>
        /// <param name="parameter">x</param>
        /// <param name="body">lambdabody</param>
        /// <returns></returns>
        private bool InvokeLambda(object v, ParameterExpression parameter, dynamic body)
        { 
            var compiledLambda1 = Expression.Lambda(body, parameter).Compile(); 
            return compiledLambda1.DynamicInvoke(DynamicCast(v, actType));   
        }

        #endregion


        #region DefineVehicle

        /// <summary>
        /// VehicleType is special
        /// Adds options
        /// do affect menu
        /// </summary>
        /// <param name="v">no use</param>
        private void VehicleType(string v)
        {
            /*  IEnumerable<Vehicle> exporters = typeof(Vehicle)
                  .Assembly.GetTypes()
                  .Where(t => t.IsSubclassOf(typeof(Vehicle)) && !t.IsAbstract)
                  .Select(t => (Vehicle)Activator.CreateInstance(t));
                  */
            // Remove A-Q options
            EmptyAddRemoveInstance();
            parameters = new List<Parameter>();
            parameterChildren = GetAllChildren(parameters.GetType().GetGenericArguments().Single());
            //item => item. < char.Q && 
            Console.WriteLine("Choose vehicle type");
            foreach (var item in vehicleKeys)
            {
                Console.WriteLine($"Press {PrettyPrint(item.Key)} \t {item.Value.Name}");
            }
            actType = GetKeyTypes(vehicleKeys);
            if (! (savedConstructors.TryGetValue(actType, out System.Reflection.ParameterInfo[] foundParameterlist)))
            { 
                savedConstructors.Add(actType, GetFirstCtorsParametes(actType));
            }
            var ctorParas = savedConstructors[actType];
            int i = Convert.ToInt32('A');
            char keychar;
            foreach (var param in ctorParas)
            {
                keychar = (char)i;
                defineVehicleSwitch.Add(keychar, AllParam);
                defineVehicleDesc.Add(keychar, param.Name);
                i++;
                if (keychar >= 'Q')
                {
                    throw new IndexOutOfRangeException("More than 18 parameters in constructor, doesn't fit (A to Q)");
                }
            }
        }


        /// <summary>
        /// Remove options at end.
        /// do affect menu
        /// </summary>
        private void EmptyAddRemoveInstance()
        {
            int i = Convert.ToInt32('A');
            char keychar;
            for (int ij = 0; ij < 16; ij++)
            {
                keychar = (char)i;
                defineVehicleDesc.Remove(keychar);
                defineVehicleSwitch.Remove(keychar);
                i++;
            }
            defineVehicleDesc.Remove('\r');
            defineVehicleSwitch.Remove('\r');
        }


        /// <summary>
        /// Askes and parsed a param, all but VehicleType
        /// doesn't affect menu
        /// </summary>
        /// <param name="v">Name_Ordinal_Type</param>
        private void AllParam(string v)
        {
            // v: Name_Ordinal_Type
            var vparts = v.Split('_');
            Type toClass=paramtoParamtype[vparts[2]];
            Parameter res = parameters.FirstOrDefault(p => p.Name == vparts[0] && p.Type == vparts[2]);
            if (res == null)
            {
                Console.WriteLine("Please enter " + vparts[0]);
                var gottenvalue = GetAll(vparts[2]);

                Parameter NewParameter = (Parameter) Activator.CreateInstance(toClass, new Object[]
                {
                   Int32.Parse(vparts[1]),
                   vparts[0],
                   vparts[2],
                   gottenvalue
                });

                parameters.Add(NewParameter);
            }
            else
            {
                var goodres = DynamicCast(res, toClass);
                Console.WriteLine("Please enter " + vparts[0] + " the last value was " + goodres.Value);    
                var gottenvalue = GetAll(vparts[2]);

                Parameter removethis = (Parameter)Activator.CreateInstance(toClass, new Object[]
                {
                    Int32.Parse(vparts[1]),
                    vparts[0],
                    vparts[2],
                    goodres.Value
                });
                parameters.Remove(removethis);

                Parameter NewParameter = (Parameter) Activator.CreateInstance(toClass, new Object[]
                {
                    Int32.Parse(vparts[1]),
                    vparts[0],
                    vparts[2],
                    gottenvalue
                });

                parameters.Add(NewParameter);
            }
            if(IsLicensePlateNumberAndTypeSet())
            {
                Type toClassLicencePlateNumber = paramtoParamtype["System.String"];
                var LicencePlateNumber = DynamicCast(parameters.FirstOrDefault(p => p.Name == "LicensePlateNumber"), toClassLicencePlateNumber).Value;
                if (!garageHandler.IsUniq(actType, LicencePlateNumber))
                {
                    Console.WriteLine($"The licenceplatenumber {LicencePlateNumber} is allready in the garage");
                    Console.WriteLine($"Press a key");
                    Console.ReadKey();
                    parameters.Remove(parameters.FirstOrDefault(p => p.Name == "LicensePlateNumber"));
                }

            }

        }

        private bool IsLicensePlateNumberAndTypeSet()
        {
           return parameters.Any(p => p.Name == "LicensePlateNumber") && actType != null;
        }



        #endregion


        #region submenu
        /// <summary>
        /// ListAllVehicles
        /// doesn't affect menu
        /// </summary>
        /// <param name="v">no use</param>
        private void ListAllVehicles(string v)
        {
            var res = garageHandler.GetAllVehicle​s();
            Console.WriteLine("");
            if (res.Length > 0)
            {
                foreach (var item in res.Where(i => i != null))
                {
                    Console.WriteLine(item);
                }
            }
            else
            {
                Console.WriteLine($"No items found");
            }
            Console.Write("Press a key");
            Console.ReadKey();
        }

        /// <summary>
        /// ListPerType
        /// doesn't affect menu
        /// </summary>
        /// <param name="v">no use</param>
        private void ListPerType(string v)
        {
            var res = garageHandler.GetVehiclesGroupby();
            Console.WriteLine("");
            if (res.Count() > 0)
            {
                foreach (var item in res.Where(i => i != null))
                {
                    Console.WriteLine($"{item.Type}\t{item.Antal}");
                }
            }
            else
            {
                Console.WriteLine($"No items found");
            }
            Console.Write("Press a key");
            Console.ReadKey();
        }

        /// <summary>
        /// Go to AddVehicle menu Fail fast här?
        /// doesn't affect menu
        /// </summary>
        /// <param name="v">no use</param>
        private void AddVehicle(string v)
        {
            if (garageHandler.IsFull)
            {
                Console.WriteLine($"Sorry the garage is full");
                Console.WriteLine("press a key");
                Console.ReadKey();
            }
            else
            {
                activeMenu = Submenus.DefineVehicle;
            }
        
        }

        /// <summary>
        /// Go to RemoveVehicle menu
        /// doesn't affect menu
        /// </summary>
        /// <param name="v">no use</param>

        private void RemoveVehicle(string v)
        {
            activeMenu = Submenus.GetSearchParameters;
            activeSearchTypes = SearchTypes.Remove;
        }

        /// <summary>
        /// Search By License Number
        /// doesn't affect menu
        /// </summary>
        /// <param name="v">no use</param>
        private void SearchByLicenseNumber(string v)
        {
            Console.WriteLine("Please write the licenseplatenumber to search for");
            string input = GetAll("System.String");

            var res = garageHandler.GetByRegNumber(input);
            if (res.Length > 0)
            {
                foreach (var item in res.Where(i=> i!= null))
                {
                    Console.WriteLine(item);
                }
            }
            else
            {
                Console.WriteLine($"No items found for licenseplatenumber {input} ");
            }
            Console.Write("Press a key");
            Console.ReadKey();
        }


        /// <summary>
        /// Go to SearchVehicle meny
        /// doesn't affect menu
        /// </summary>
        /// <param name="v">No use</param>
        private void SearchVehicle(string v)
        {
            activeMenu = Submenus.GetSearchParameters;
            searchRes = garageHandler.GetAllVehicles();
            activeSearchTypes = SearchTypes.Search;

        }


        /// <summary>
        /// LoadMock
        /// doesn't affect menu
        /// </summary>
        /// <param name="obj">no use</param>
        private void LoadMock(string obj)
        {
            garageHandler.Insertmock();
            searchRes = garageHandler.GetAllVehicles()

         ;
            Console.WriteLine("Mock loaded");
            Console.Write("Press a key");
            Console.ReadKey();
        }

        /// <summary>
        /// Resize Garage
        /// doesn't affect menu
        /// </summary>
        /// <param name="v">no use</param>
        private void ResizeGarage(string v)
        {
            Console.WriteLine("Please assing a new capaticy for the garage");
            int capacity = -1;

            while (capacity < 0)
            {
                capacity = GetAll("System.Int32");
            }

            var res = garageHandler.ResizeGarage(capacity);
            if (res.Count() > 0)
            {
                Console.WriteLine("");
                Console.WriteLine("These vehicles didn't fit and are deleted");
                foreach (var item in res.Where(i => i != null))
                {
                    Console.WriteLine(item);
                }
                Console.Write("Press a key");
                Console.ReadKey();
            }
        }




        /// <summary>
        /// Save json
        /// doesn't affect menu
        /// </summary>
        /// <param name="v">no use</param>
        private void SaveGarage(string v)
        {
            Console.WriteLine("Please provide path for the file to save");
            activeFileType = FileTypes.Save;
             ProcessPath();
        }
        #endregion

        #region MainMenu
        /// <summary>
        /// BuildGarage asks for size, and some vars
        /// do affect globals
        /// </summary>
        /// <param name="v">no use</param>
        private void BuildGarage(string v)
        {
            Console.WriteLine("Please assign the capaticy for the garage");

            int capacity = -1;

            while (capacity < 0)
            {
                capacity = GetAll("System.Int32");
                if (capacity<0)
                    Console.WriteLine($"Capacity needes to be positive, {capacity} isn't that");
            }
            garageHandler.CreateNewGarage(capacity);
            // now the garage is defined....

            
            subType = garageHandler.GetSubType();
            GarageChildren = GetAllChildren(subType);

            int i = Convert.ToInt32('A');
            char key;
            foreach (var item in GarageChildren)
            {
                key = (char)i;
                vehicleKeys.Add(key, item);
                i++;
            }
           searchRes = garageHandler.GetAllVehicles()

                    ;

           
            activeMenu = Submenus.Second;

        }

        /// <summary>
        /// Load json
        /// doesn't affect menu
        /// </summary>
        /// <param name="v">no use</param>
        private void LoadGarage(string v)
        {
            Console.WriteLine("Please provide path for the saved file");
            activeFileType = FileTypes.Load;
            garageHandler.CreateNewGarage(1);
            ProcessPath();


            // now the garage is defined....


            subType = garageHandler.GetSubType();
            GarageChildren = GetAllChildren(subType);

            int i = Convert.ToInt32('A');
            char key;
            foreach (var item in GarageChildren)
            {
                key = (char)i;
                vehicleKeys.Add(key, item);
                i++;
            }
            searchRes = garageHandler.GetAllVehicles()

                     ;


            activeMenu = Submenus.Second;
        }

        #endregion

        #region global options
        /// <summary>
        /// Adds new , Removes serachfor or search is done here
        /// do affect menu
        /// </summary>
        /// <param name="nostring">no use</param>
        private void Done(string nostring)
        {
            if(activeMenu==Submenus.DefineVehicle)
            {

                var tempparaarray = parameters.OrderBy(v => v.Ordinal).Select(v => v.Ordinal).ToArray<int>();
                object[]  ParamArray = new Object[tempparaarray.Length];
                for (int i = 0; i < tempparaarray.Length; i++)
                {
                    var Res =parameters.Where(v => v.Ordinal == tempparaarray[i]).FirstOrDefault();
                    // obout to be the right type, but not, how to do dymnic?
                   
                   // var Children= GetAllChildren(Res.Type());

                    foreach (var Child in parameterChildren)
                    {
                        dynamic tores=new {Value="" };

                        try
                        {
                            tores = DynamicCast(Res, Child);
                        }
                        catch (Exception )
                        {
                            continue;
                          //  throw;
                        }
                        ParamArray[i] = tores.Value;
                        break;
                    }

                }

                var element = Activator.CreateInstance(actType, ParamArray);
         
                    if (!(garageHandler.AddElement(element, actType)))
                    {
                        Console.WriteLine("The garage is full");
                    }
                    else
                    {
                        actType = null;
                        parameters = new List<Parameter>();
                        EmptyAddRemoveInstance();
                    }
                searchRes = garageHandler.GetAllVehicles();

            }
            else if (activeMenu == Submenus.GetSearchParameters)
            {
                if(activeSearchTypes==SearchTypes.Search)
                { 
                    if (searchRes.Count() > 0)
                    {
                        foreach (var item in searchRes)
                        {
                            Console.WriteLine(item);
                        }
                    }
                    else
                    {
                        Console.WriteLine("Didn't find anything");
                    }

                    
                }
                else if(activeSearchTypes==SearchTypes.Remove)
                {
                    if (searchRes.Count() > 0)
                    {
                        foreach (var item in searchRes)
                        {
                            garageHandler.RemoveElement(item, item.GetType());
                        }
                     
                    }
                    else
                    {
                        Console.WriteLine("Didn't find anything to remove");
                    }
                }
                else
	            {
                    throw new ArgumentOutOfRangeException("activeSearchTypes");
                }
                searchPredicateString = "(v =>";
                startOnExpr = 0;
                searchRes = garageHandler.GetAllVehicles()
                .Where(p => (p != null));
                EmptySearchOptions();

                Console.Write("Press a key");
                Console.ReadKey();
                /*
                searchPredicateString += ")";
                searchPredicateString;
                Func<Vehicle, bool> delg = expr.Compile();
                */



            }
            else
	        {
                throw new ArgumentOutOfRangeException();
            }
        }


        /// <summary>
        /// Quit
        /// doesn't affect menu
        /// </summary>
        /// <param name="v">no use</param>
        private void Quit(string v)
        {
            inLoop = false;
        }


        /// <summary>
        /// Go Up in the menus
        /// do affect menu
        /// </summary>
        /// <param name="v">no use</param>
        private void GoUp(string v)
        {
            switch (activeMenu)
            {
                case Submenus.Second:
                    activeMenu = Submenus.Main;
                    break;
                case Submenus.DefineVehicle:
                    activeMenu = Submenus.Second;
                    parameters = new List<Parameter>();
                    actType = null;
                    EmptyAddRemoveInstance();
                    break;
                case Submenus.GetSearchParameters:
                    activeMenu = Submenus.Second;
                    searchPredicateString = "(v =>";
                    startOnExpr = 0;
                    searchRes = garageHandler.GetAllVehicles()
                    .Where(p => (p != null));
                    EmptySearchOptions();
                    break;
                default:
                    break;
            }
        }
        #endregion

        #region The menus
        /// <summary>
        /// Mainloop
        /// do affect menu
        /// </summary>
        public void WriteMeny()
        {
            var Thismenu = supeMenu[activeMenu];
            string workstring="";
            Action<string> action;
            while (inLoop)
            {
                Console.Clear();
                if (activeMenu == Submenus.DefineVehicle )
                {
                    // check if we shall to up done...
                    if (actType != null)
                    {
                        FillConstructordirectory();
                        var ctorparas = savedConstructors[actType];
                        if (parameters.Count == ctorparas.Count() && parameters.Count > 0)
                        {
                            if (!(defineVehicleSwitch.ContainsKey('\r')))
                            {
                                defineVehicleSwitch.Add('\r', Done);
                                defineVehicleDesc.Add('\r', "Done");
                            }
                            // DefineVehicleDesc = (SortedDictionary<char, string>)DefineVehicleDesc.OrderBy(item => item.Key.ToString());
                        }
                    }
                    var thisdesc = defineVehicleDesc;
                    if (parameters != null && parameters.Count>0)
                    { // skriv ut angive pars
                        Console.WriteLine($"Already stated parameters");
                        if(actType!=null)
                            Console.WriteLine($"Choosed type:\t{actType.Name}");
                        foreach (var item in parameters.OrderBy(para => para.Ordinal))
                        {

                           // var Children = GetAllChildren(item.Type());

                            foreach (var Child in parameterChildren)
                            {
                                dynamic tores = new { Value = "",Name= "" };

                                try
                                {
                                    tores = DynamicCast(item, Child);
                              
                                }
                                catch (Exception)
                                {
                                    continue;
                                }
                                Console.WriteLine($"{ tores.Name }\t{ tores.Value }");
                                break;
                            }

                            
                        }
                        
                    }
                    else if (actType != null)
                    {
                        Console.WriteLine($"Already stated parameters");
                        Console.WriteLine($"Choosed type:\t{actType.Name}");
                    }
                    Console.WriteLine("");
                    foreach (var item in thisdesc)
                    {
                        Console.WriteLine($" Press {PrettyPrint(item.Key)} for {item.Value.ToString()}");
                    }
                    Console.WriteLine("");
                    var key = char.ToUpper(Console.ReadKey().KeyChar);
                    while (!(Thismenu.TryGetValue(key, out action)))
                    {
                        Console.WriteLine($"undefined key: {PrettyPrint(key)}, try again");
                    key = char.ToUpper(Console.ReadKey().KeyChar);
                    }
                    // v: Name_Ordinal_Type
                   
                    if (actType != null &&  key <'Q'  && key>= 'A')
                    {
                        

                        var name = thisdesc[key];
                        /* var ctors = actType.GetConstructors();
                         var ctor = ctors[0];
                         */
                        FillConstructordirectory();

                        var ctorparas = savedConstructors[actType];
                        var ordinal = ctorparas.Where(para => para.Name == name).Select(para => para.Position).FirstOrDefault();
                        var paratype= ctorparas.Where(para => para.Name == name).Select(para => para.ParameterType).FirstOrDefault();
                        workstring = name + "_" + ordinal + "_" + paratype.ToString();
                        
                      
                        // check if we shall to up done...
                        if (parameters.Count == ctorparas.Count() && parameters.Count >0)
                        {
                            defineVehicleSwitch.Add('\r', Done);
                            defineVehicleDesc.Add('\r', "Done");

                           // DefineVehicleDesc = (SortedDictionary<char, string>)DefineVehicleDesc.OrderBy(item => item.Key.ToString());
                        }
                    }
        



                }
                else
                {
                    if (activeMenu == Submenus.GetSearchParameters)
                    {
                        Console.WriteLine($"The query now {searchPredicateString + ")"}");
                        Console.WriteLine("");
                        if(activeSearchTypes==SearchTypes.Remove)
                            Console.WriteLine($"\t number of hits {searchRes.Count()}");
                            var thisdesc = getSearchParametersDesc;
                        foreach (var item in thisdesc)
                        {
                            Console.WriteLine($" Press {PrettyPrint(item.Key)} for {item.Value}");
                        }
                        Console.WriteLine("");
                        var key = char.ToUpper(Console.ReadKey().KeyChar);

                        
                        while (!Thismenu.TryGetValue(key, out action))
                        {
                            Console.WriteLine($"undefined key: {PrettyPrint(key)}, try again");
                            key = char.ToUpper(Console.ReadKey().KeyChar);
                        }

                        if (actType != null && key < 'Q' && key >= 'E')
                        {
                            FillConstructordirectory();
                            var ctorparas = savedConstructors[actType];
                            var name = thisdesc[key].Replace("Search for ","");
                            var ordinal = ctorparas.Where(para => para.Name == name).Select(para => para.Position).FirstOrDefault();
                            var paratype = ctorparas.Where(para => para.Name == name).Select(para => para.ParameterType).FirstOrDefault();
                            workstring = name + "_" + ordinal + "_" + paratype.ToString();
                        }
                    }
                    else
                    {
                        foreach (var item in Thismenu)
                        {
                            Console.WriteLine($" Press {PrettyPrint(item.Key)} for {item.Value.Method.Name.ToString()}");
                        }
                        Console.WriteLine("");
                        var key = char.ToUpper(Console.ReadKey().KeyChar);
                        while (!(Thismenu.TryGetValue(key, out action)))
                        {
                            Console.WriteLine($"undefined key: {PrettyPrint(key)}, try again");
                            key = char.ToUpper(Console.ReadKey().KeyChar);
                        }
                        
                    }


                }
                Console.WriteLine("");
                action(workstring);
                Thismenu = supeMenu[activeMenu];
            }
    
        }

        /// <summary>
        /// Fills SavedConstructors for actType
        /// doesn't affect menu
        /// </summary>
        private void FillConstructordirectory()
        {
            if (!(savedConstructors.TryGetValue(actType, out System.Reflection.ParameterInfo[] foundParameterlist)))
            {
                savedConstructors.Add(actType, GetFirstCtorsParametes(actType));
            }
        }


        #endregion
    }
}
