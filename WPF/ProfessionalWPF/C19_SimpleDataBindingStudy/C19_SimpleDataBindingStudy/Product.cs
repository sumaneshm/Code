using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace C19_SimpleDataBindingStudy
{
    public class Product : INotifyPropertyChanged
    {
        public int ProductId { get; set; }

        private string modelNumber;
        public string ModelNumber { get { return modelNumber; } set { modelNumber = value; OnPropertyChanged(new PropertyChangedEventArgs("ModelNumber")); } }

        private string modelName;
        public string ModelName { get { return modelName; } set { modelName = value; OnPropertyChanged(new PropertyChangedEventArgs("ModelName")); } }

        private decimal unitPrice;
        public decimal UnitPrice { get { return unitPrice; } set { unitPrice = value; OnPropertyChanged(new PropertyChangedEventArgs("UnitPrice")); } }

        private string description;
        public string Description { get { return description; } set { description = value; OnPropertyChanged(new PropertyChangedEventArgs("Description")); } }

        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, e);
        }
    }
}
