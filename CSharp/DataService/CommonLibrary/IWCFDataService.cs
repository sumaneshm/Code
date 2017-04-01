using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace CommonLibrary
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IWCFDataService" in both code and config file together.
    [ServiceContract]
    [ServiceKnownType(typeof(Employee))]
    [ServiceKnownType(typeof(Student))]
    public interface IWCFDataService
    {

        [OperationContract]
        void Add(ManagableElement element);

        [OperationContract]
        void Edit(ManagableElement element);

    }
}
