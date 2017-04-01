using DICollege_Common;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DICollege_SQL
{
    public class SQLCollegeRepository : ICollegeRepository
    {

        private string _connectionString;

        public SQLCollegeRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public IEnumerable<Student> GetAllStudents()
        {
            List<Student> students = new List<Student>();

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "Get_Students";
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;

                    using(SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            students.Add(
                                new Student
                                {
                                    Name = reader["Name"].ToString(),
                                    Age = Int32.Parse(reader["Age"].ToString()),
                                    ID = Int32.Parse(reader["ID"].ToString()),
                                    Roll = reader["Roll"].ToString()
                                });
                        }
                    }

                }
            }

            return students;
        }

        public void InsertStudent(Student studentToInsert)
        {
            throw new NotImplementedException();
        }

        public void DeleteStudent(int id)
        {
            throw new NotImplementedException();
        }
    }
}
