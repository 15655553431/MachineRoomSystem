
 using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IDal
{
    public interface IDbSession
    {
    	IComputerinfoDal ComputerinfoDal { get;}
    
    	IMachineroomDal MachineroomDal { get;}
    
    	INewsinfoDal NewsinfoDal { get;}
    
    	IReservationDal ReservationDal { get;}
    
    	IUserinfoDal UserinfoDal { get;}
    
	    int SaveChanges();
    }
}