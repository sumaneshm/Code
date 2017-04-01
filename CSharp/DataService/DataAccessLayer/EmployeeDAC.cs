using CommonLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer
{
    public class EmployeeDAC
    {
        public void Add(Employee employee)
        {
            Console.WriteLine("Call Employee Add SP and pass name : {0}, Roll number : {1}",employee.Name, employee.RollNumber);
        }

        public void Edit(Employee employee)
        {
            Console.WriteLine("Call Employee Edit SP and pass name : {0}, Roll number : {1}", employee.Name, employee.RollNumber);
        }
    }
}
