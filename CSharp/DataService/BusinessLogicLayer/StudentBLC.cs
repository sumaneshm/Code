using CommonLibrary;
using DataAccessLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BusinessLogicLayer
{
    public class StudentBLC : IBusinessLogic<Student, Student>
    {
        public StudentBLC()
        {
            Console.WriteLine("Student BLC is getting created");
        }

        public void Add(Student element)
        {
            Console.WriteLine("Perform your business logic specific to Student here for add case");
            
            StudentDAC dac = new StudentDAC();

            dac.Add(element);
        }

        public void Edit(Student element)
        {
            Console.WriteLine("Perform your business logic specific to Student here for edit case");
            Student e = element as Student;
            StudentDAC dac = new StudentDAC();

            dac.Edit(element);
        }
        

        public Student Get(int i)
        {
            return new Student();
        }
    }
}
