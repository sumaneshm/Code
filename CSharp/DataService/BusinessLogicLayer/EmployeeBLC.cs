using CommonLibrary;
using DataAccessLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer
{
    //public abstract class BaseBLC<T> : IBusinessLogic<T,O> where T : ManagableElement
    //{
    //    public void Add(ManagableElement element)
    //    {
    //        Add((T)element);
    //    }

    //    public void Edit(ManagableElement element)
    //    {
    //        Edit((T)element);
    //    }
        
    //    public abstract void Add(T element);

    //    public abstract void Edit(T element);
    //}

    public class EmployeeBLC: IBusinessLogic<Employee,Employee>
    {
        public EmployeeBLC()
        {
            Console.WriteLine("New EmployeeBLC is created");
        }

        //public override void Add(Employee element)
        //{
        //    EmployeeDAC dac = new EmployeeDAC();

        //    dac.Add(element);
        //}

        //public override void Edit(Employee element)
        //{
        //    EmployeeDAC dac = new EmployeeDAC();

        //    dac.Edit(element);
        //}



        public void Add(Employee element)
        {
            Console.WriteLine("Adding a new employee");
        }

        public void Edit(Employee element)
        {
            Console.WriteLine("Editing a new employee");
        }

        public Employee Get(int i)
        {
            return new Employee();
        }

      
    }
}
