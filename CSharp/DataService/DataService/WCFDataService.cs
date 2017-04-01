using BusinessLogicLayer;
using CommonLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace DataService
{

    public class WCFDataService : IWCFDataService
    {
        public void Add(ManagableElement element)
        {
            new BusinessManager().Add(element);
        }
        
       public void Edit(ManagableElement element)
        {
            new BusinessManager().Edit(element);
        }
    }
}