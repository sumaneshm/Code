using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Data;
using System.Windows.Media;

namespace TreezMVVM.Converters
{
    class NumberTypeColorConverter : IValueConverter
    {
        public Brush ChorusColor { set; get; }

        public Brush GFRMColor { get; set; }

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            bool showChorus = (bool)value;

            if (showChorus)
                return ChorusColor;
            else
                return GFRMColor;

        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
