using CommonLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer
{
    //public interface IBusinessLogic
    //{
    //    void Add(ManagableElement element);
    //    void Edit(ManagableElement element);
    //    ManagableElement Get(int i);
    //}

    public interface IBusinessLogic<in TI, out TO>  where TO : TI
    {
        void Add(TI element);
        void Edit(TI element);

        TO Get(int i);
    }
}
