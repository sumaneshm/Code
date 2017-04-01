using CommonLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer
{
    public class StudentDAC
    {
        public void Add(Student student)
        {
            Console.WriteLine("Call Student Add SP and pass name : {0}, class : {1}", student.StudentName, student.Class);
        }

        public void Edit(Student student)
        {
            Console.WriteLine("Call Student Edit SP and pass name : {0}, class : {1}", student.StudentName, student.Class);
        }
    }
}
