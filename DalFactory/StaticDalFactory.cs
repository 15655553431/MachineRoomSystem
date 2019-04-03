
 using IDal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace DalFactory
{
    public class StaticDalFactory
    {
		public static string assemblyName = System.Configuration.ConfigurationManager.AppSettings["DalAssemblyName"];
		public static IComputerinfoDal GetComputerinfoDal()
        {
            return Assembly.Load(assemblyName).CreateInstance(assemblyName+".ComputerinfoDal") as IComputerinfoDal;
        }
	

		public static IMachineroomDal GetMachineroomDal()
        {
            return Assembly.Load(assemblyName).CreateInstance(assemblyName+".MachineroomDal") as IMachineroomDal;
        }
	

		public static INewsinfoDal GetNewsinfoDal()
        {
            return Assembly.Load(assemblyName).CreateInstance(assemblyName+".NewsinfoDal") as INewsinfoDal;
        }
	

		public static IReservationDal GetReservationDal()
        {
            return Assembly.Load(assemblyName).CreateInstance(assemblyName+".ReservationDal") as IReservationDal;
        }
	

		public static IUserinfoDal GetUserinfoDal()
        {
            return Assembly.Load(assemblyName).CreateInstance(assemblyName+".UserinfoDal") as IUserinfoDal;
        }
	

	}
}


