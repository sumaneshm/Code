using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Data;
using TreeLibrary;
using System.Windows.Media;

namespace TreezMVVM.Converters
{
    class NodeTypeBrushConverter : IValueConverter
    {
        public Brush OfficialColor { get; set; }
        public Brush GenericColor { get; set; }
        public Brush ArtificialColor { get; set; }

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            NodeType nType = (NodeType)value;
            switch (nType)
            {
                case NodeType.Generic:
                    return GenericColor;
                case NodeType.Artificial:
                    return ArtificialColor;
                default:
                    return OfficialColor;
            }

        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
