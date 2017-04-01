using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

namespace NewResourceDictionary
{
    public class NewCustomResources
    {
        public static ComponentResourceKey SadTileBrush
        {
            get
            {
                return new ComponentResourceKey(
                    typeof(NewCustomResources), "SadTileBrush");
            }
        }
    }
}
