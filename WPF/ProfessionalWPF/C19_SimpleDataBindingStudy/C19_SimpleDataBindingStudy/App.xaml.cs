using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Windows;

namespace C19_SimpleDataBindingStudy
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private static StoreDB Store = new StoreDB();

        public static StoreDB StoreDB
        {
            get { return Store; }
        }


    }
}
