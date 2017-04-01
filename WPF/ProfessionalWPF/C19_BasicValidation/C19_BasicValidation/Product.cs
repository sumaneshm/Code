using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace C19_BasicValidation
{
    public class Product : INotifyPropertyChanged, IDataErrorInfo
    {
        public int ProductId { get; set; }

        private string modelNumber;
        public string ModelNumber { get { return modelNumber; } set { modelNumber = value; OnPropertyChanged(new PropertyChangedEventArgs("ModelNumber")); } }

        private string modelName;
        public string ModelName { get { return modelName; } set { modelName = value; OnPropertyChanged(new PropertyChangedEventArgs("ModelName")); } }

        private decimal unitPrice;
        public decimal UnitPrice
        {
            get { return unitPrice; }
            set
            {
                if (value < 0)
                    throw new ArgumentException("UnitPrice cannot be negative");
                else
                {
                    unitPrice = value;
                    OnPropertyChanged(new PropertyChangedEventArgs("UnitPrice"));
                }
            }
        }

        private string description;
        public string Description { get { return description; } set { description = value; OnPropertyChanged(new PropertyChangedEventArgs("Description")); } }

        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, e);
        }

        public string Error
        {
            get { throw null; }
        }

        public string this[string columnName]
        {
            get {
                if (columnName == "ModelNumber")
                {
                    foreach(char ch in modelNumber)
                    {
                        if (!Char.IsLetterOrDigit(ch))
                            return "ModelNumber should only contain letters and numbers";
                    }
                }
                return null;
            }
        }
    }
}
