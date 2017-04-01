using System;
using System.Windows.Media;
using System.Globalization;
using System.Windows.Data;

namespace C20_SampleValueConverter
{
    class MyValueConverter : IValueConverter
    {

        public Brush StudentColor { set; get; }

        public Brush TeacherColor { get; set; }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            DType objType = (DType) value;
            if (objType == DType.STUDENT)
                return StudentColor;
            else
                return TeacherColor;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
