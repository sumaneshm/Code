using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

namespace C20_SampleValueConverter
{
    public enum DType
    {
        STUDENT,
        TEACHER
    }
    public class MyDataItem : DependencyObject
    {
        //public static readonly DependencyProperty TitleProperty =
        //    DependencyProperty.Register("Title", typeof (string), typeof (MyDataItem), new UIPropertyMetadata(""));

        //public string Title
        //{
        //    get { return (string)GetValue(TitleProperty); }
        //    set { SetValue(TitleProperty, value); }
        //}

        //// Using a DependencyProperty as the backing store for Title.  This enables animation, styling, binding, etc...
        //public static readonly DependencyProperty TitleProperty =
        //    DependencyProperty.Register("Title", typeof(string), typeof(ownerclass), new UIPropertyMetadata(0));
        
        public string Title
        {
            get { return (string)GetValue(TitleProperty); }
            set { SetValue(TitleProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Title.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty TitleProperty =
            DependencyProperty.Register("Title", typeof(string), typeof(MyDataItem), new UIPropertyMetadata(""));

        public string StringType { get; set; }


        public DType MyType { get; set; }

        //// Using a DependencyProperty as the backing store for MyType.  This enables animation, styling, binding, etc...
        //public static readonly DependencyProperty MyTypeProperty =
        //    DependencyProperty.Register("MyType", typeof(DType), typeof(MyDataItem), new UIPropertyMetadata(0));


        public MyDataItem(string title, DType dType)
        {
            this.Title = title;
            this.MyType = dType;
        }

        public MyDataItem(string title, string strType)
        {
            this.Title = title;
            this.StringType = strType;
        }



    }
}
