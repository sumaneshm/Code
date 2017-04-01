using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace CommonLibrary
{
     [DataContract]
    public class Student : ManagableElement
    {
         [DataMember]
         public string StudentName { get; set; }

         [DataMember]
         public string Class { get; set; }
    }
}
