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

namespace C9_RedoUndoFunctionality
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private static RoutedUICommand applicationRedoCommand;
        public static RoutedUICommand ApplicationRedoCommand
        {
            get
            {
                return applicationRedoCommand;
            }
        }


        private static RoutedUICommand applicationUndoCommand;
        public static RoutedUICommand ApplicationUndoCommand
        {
            get
            {
                return applicationUndoCommand;
            }
        }

        static MainWindow()
        {
            applicationRedoCommand = new RoutedUICommand("ApplicationRedo", "Application Redo", typeof(MainWindow));
            applicationUndoCommand = new RoutedUICommand("ApplicationUndo", "Application Undo", typeof(MainWindow));
        }

        public MainWindow()
        {
            InitializeComponent();
            this.AddHandler(CommandManager.PreviewExecutedEvent, new ExecutedRoutedEventHandler(PreviewExecuted));
            this.CommandBindings.Add(new CommandBinding(MainWindow.ApplicationRedoCommand, ApplicationRedo_Executed, ApplicationRedo_CanExecute));
            this.CommandBindings.Add(new CommandBinding(MainWindow.ApplicationUndoCommand, ApplicationUndo_Executed, ApplicationUndo_CanExecute));
        }

        public void PreviewExecuted(object sender, ExecutedRoutedEventArgs args)
        {
            if (args.Source is ICommandSource) return;
            if (args.Command == MainWindow.ApplicationRedoCommand || args.Command == MainWindow.ApplicationUndoCommand) return;

            RoutedCommand cmd = (RoutedCommand)args.Command;
            if (args.Source is TextBox)
            {
                TextBox txt = (TextBox)args.Source;
                lstUndoCommands.Items.Insert(0,new CommandHistoryItem(cmd.Name, (UIElement)args.Source, "Text", ((TextBox)args.Source).Text));
            }
        }

        public void ApplicationRedo_Executed(object sender, ExecutedRoutedEventArgs e)
        {
           
            
        }

        public void ApplicationRedo_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            //Redo command can not be executed...
            e.CanExecute = false;// lstRedoCommands != null && lstRedoCommands.Items.Count > 0;
        }

        public void ApplicationUndo_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            CommandHistoryItem item = (CommandHistoryItem)lstUndoCommands.Items[0];
            item.Undo();
            lstUndoCommands.Items.RemoveAt(0);
            lstRedoCommands.Items.Insert(0, item);

        }

        public void ApplicationUndo_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = lstUndoCommands != null && lstUndoCommands.Items.Count > 0;
        }
    }
}
