using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace WpfChartAndRibbonStudy
{
    static class CustomCommands
    {
        public static RoutedCommand ShowRecentRequests = new RoutedCommand();
        public static RoutedCommand ShowSearch = new RoutedCommand();
        public static RoutedCommand ShowSummaryView = new RoutedCommand();
        public static RoutedCommand EditEnvironment = new RoutedCommand();
        public static RoutedCommand EditPreference = new RoutedCommand();
    }
}
