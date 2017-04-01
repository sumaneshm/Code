using System;
using System.Xml.Serialization;

namespace ExploreFluentAssertions.Business
{
    public class BaseClass
    {
        
    }

    public class DerivedClass
    {
        
    }


    public class Member
    {
        public string Name { get; set; }
        public string Relationship { get; set; }
    }

    [Serializable]
    public class Student 
    {
        public string Name { get; set; }

        public string RollNumber { get; set; }

        public int Age { get; set; }

        [NonSerialized]
        private string personalInfo;

        public string PersonalInfo
        {
            get { return personalInfo; }
            set { personalInfo = value; }
        }

        [XmlIgnore]
        public string SomeOtherField;
        
        public override bool Equals(object obj)
        {
            Student other = obj as Student;
            return other != null && other.Age == Age && other.Name == Name;// && base.Equals(obj);
        }
    }
}
