﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DICollege_Common
{
    public interface ICollegeRepository
    {
        IEnumerable<Student> GetAllStudents();

        void InsertStudent(Student studentToInsert);

        void DeleteStudent(int id);
    }
}
