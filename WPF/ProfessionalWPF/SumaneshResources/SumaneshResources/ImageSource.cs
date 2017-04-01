using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

namespace SumaneshResources
{
    public class ImageSource
    {
        public static ComponentResourceKey SadFaceImage
        {
            get
            {
                return new ComponentResourceKey(typeof(ImageSource), "SadFaceBrush");
            }
        }
    }
}
