using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace CommonLibrary
{
     [DataContract]
    public class Employee : ManagableElement
    {

         [DataMember]
        public string Name { get; set; }

         [DataMember]
        public int RollNumber { get; set; }
    }
}
