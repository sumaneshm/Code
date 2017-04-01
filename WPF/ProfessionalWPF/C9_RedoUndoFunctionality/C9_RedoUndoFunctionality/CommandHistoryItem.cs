using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Reflection;

namespace C9_RedoUndoFunctionality
{
    public class CommandHistoryItem
    {
        public string CommandName { get; set; }
        public UIElement CommandSource { get; set; }
        public string PropertyName { get; set; }
        public object PreviousValue { get; set; }

        public CommandHistoryItem(string commandName, UIElement source, string propertyName, object previousValue)
        {
            CommandName = commandName;
            CommandSource = source;
            PropertyName = propertyName;
            PreviousValue = previousValue;
        }

        public bool CanUndo
        {
            get
            {
                return CommandSource != null && PreviousValue != null;
            }
        }

        public void Undo()
        {
            if (CanUndo)
            {
                Type type = CommandSource.GetType();
                PropertyInfo prop = type.GetProperty(PropertyName);
                prop.SetValue(CommandSource,PreviousValue,null);
            }
        }
    }
}
