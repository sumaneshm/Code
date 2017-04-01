using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows.Data;
using System.Windows.Media;

namespace TreeViewWithViewModelDemo.TextSearch.ViewModel
{
    [ValueConversion(typeof(int),typeof(Brush))]
    public class AgeColorConverter : IValueConverter
    {
        public Brush MajorColor { get; set; }

        public Brush MinorColor { get; set; }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            int age = (int) value;

            if (age <= 30)
            {
                return MinorColor;
            }
            else
            {
                return MajorColor;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
