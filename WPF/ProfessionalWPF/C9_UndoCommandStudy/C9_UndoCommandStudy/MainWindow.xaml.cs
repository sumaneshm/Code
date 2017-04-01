using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace C9_UndoCommandStudy
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private static RoutedUICommand applicationUndo;

        static MainWindow()
        {
            applicationUndo = new RoutedUICommand("ApplicationUndo", "Application Undo", typeof(MainWindow));
        }

        public static RoutedUICommand ApplicationUndo
        {
            get { return applicationUndo; }
        }

        public MainWindow()
        {
            InitializeComponent();
            this.AddHandler(CommandManager.PreviewExecutedEvent,new ExecutedRoutedEventHandler(Executed));
        }

        private void Executed(object sender, ExecutedRoutedEventArgs e)
        {
            if (e.Source is ICommandSource) return;

            if (e.Command == MainWindow.ApplicationUndo) return;

            TextBox txt = e.Source as TextBox;
            if (txt != null)
            {
                RoutedCommand cmd = (RoutedCommand)e.Command;
                CommandHistoryItem item = new CommandHistoryItem(cmd.Name, txt, "Text", txt.Text);
                lstCommands.Items.Insert(0, item);
            }

        }

        private void CommandBinding_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute= lstCommands != null && lstCommands.Items.Count > 0;
        }

        private void CommandBinding_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            var cmd = (CommandHistoryItem)lstCommands.Items[0];
            cmd.Undo();
            lstCommands.Items.RemoveAt(0);
        }
    }
}
