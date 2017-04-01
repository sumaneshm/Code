using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

namespace C9_UndoCommandStudy
{
    public class CommandHistoryItem
    {
        public string CommandName { get; set; }

        public UIElement ElementActedOn { get; set; }

        public string PropertyName { get; set; }

        public object PreviousStatus { get; set; }


        public CommandHistoryItem(string CommandName, UIElement ElementActedOn, string PropertyName, object PreviousStatus)
        {
            this.CommandName = CommandName;
            this.ElementActedOn = ElementActedOn;
            this.PropertyName = PropertyName;
            this.PreviousStatus = PreviousStatus;
        }

        public bool CanUndo
        {
            get
            {
                return (ElementActedOn != null && PropertyName != null);
            }
        }

        public void Undo()
        {
            Type elementType = ElementActedOn.GetType();
            var prop = elementType.GetProperty(PropertyName);
            prop.SetValue(ElementActedOn, PreviousStatus, null);
        }
    }
}
