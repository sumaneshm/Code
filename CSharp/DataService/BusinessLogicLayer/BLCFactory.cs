using CommonLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer
{
    public static  class BLCFactory
    {
        public static IBusinessLogic<ManagableElement, ManagableElement> GetBLC(ManagableElement add)
        {
            switch(add.GetType().ToString())
            {
                case "CommonLibrary.Employee":
                    return new EmployeeBLC();
                default:
                    return new StudentBLC();
            }
            
        }   
    }
}
