using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;

namespace CustomRoutedUICommandStudy
{
    class MyFirstCommand
    {
        private static RoutedUICommand command;

        static MyFirstCommand()
        {
            InputGestureCollection inputs = new InputGestureCollection();
            inputs.Add(new KeyGesture(Key.R,ModifierKeys.Control,"Ctrl+R"));
            command = new RoutedUICommand("Requery", "Requery", typeof(MyFirstCommand), inputs);
        }

        public static RoutedUICommand Command
        {
            get { return command; }
        }
    }
}
