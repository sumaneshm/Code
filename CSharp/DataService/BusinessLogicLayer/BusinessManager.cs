using CommonLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer
{

    public class BusinessManager
    {
        public void Add(ManagableElement element)
        {
            BLCFactory.GetBLC(element).Add(element);
        }
        
        public void Edit(ManagableElement element)
        {
            BLCFactory.GetBLC(element).Edit(element);
        }

        public ManagableElement Get<T>(int id) where T : ManagableElement
        {
            return BLCFactory.GetBLC(default(T)).Get(id);
        }
    }
}
