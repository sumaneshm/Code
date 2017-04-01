using System.Collections.Generic;

namespace BusinessLib
{
    /// <summary>
    /// A simple data transfer object (DTO) that contains raw data about a person.
    /// </summary>
    public class Person
    {
        readonly List<Person> _children = new List<Person>();

        public IList<Person> Children
        {
            get { return _children; }
        }

        public string Name { get; set; }

        public int Age { set; get; }

        public string RollNumber { get; set; }

        public string ExaminationNumber { get; set; }
    }
}