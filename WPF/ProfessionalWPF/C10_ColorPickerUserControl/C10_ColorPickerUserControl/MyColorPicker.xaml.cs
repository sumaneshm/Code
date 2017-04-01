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

namespace C10_ColorPickerUserControl
{
    /// <summary>
    /// Interaction logic for MyColorPicker.xaml
    /// </summary>
    public partial class MyColorPicker : UserControl
    {
        public MyColorPicker()
        {
            InitializeComponent();
            SetupCommands();
        }

        private void SetupCommands()
        {
            //CommandBinding binding = new CommandBinding(ApplicationCommands.Undo, UndoCommand_Executed, UndoCommand_CanExecute);
            //this.CommandBindings.Add(binding);
        }

        private Color? previousColor;

        public static void UndoCommand_Executed(object sender, ExecutedRoutedEventArgs args)
        {
            MyColorPicker cpk = (MyColorPicker)sender;
            cpk.Color = (Color)cpk.previousColor;
        }

        public static void UndoCommand_CanExecute(object sender, CanExecuteRoutedEventArgs args)
        {
            MyColorPicker cpk = (MyColorPicker)sender;
            args.CanExecute = cpk.previousColor.HasValue;
        }

        static MyColorPicker()
        {
            ColorProperty = DependencyProperty.Register("Color", typeof(Color), typeof(MyColorPicker), new PropertyMetadata(new PropertyChangedCallback(OnColorChanged)));
            RedProperty = DependencyProperty.Register("Red", typeof(byte), typeof(MyColorPicker), new PropertyMetadata(new PropertyChangedCallback(OnColorRGBChanged)));
            BlueProperty = DependencyProperty.Register("Blue", typeof(byte), typeof(MyColorPicker), new PropertyMetadata(new PropertyChangedCallback(OnColorRGBChanged)));
            GreenProperty = DependencyProperty.Register("Green", typeof(byte), typeof(MyColorPicker), new PropertyMetadata(new PropertyChangedCallback(OnColorRGBChanged)));


            CommandManager.RegisterClassCommandBinding(typeof(MyColorPicker),
             new CommandBinding(ApplicationCommands.Undo,
             UndoCommand_Executed, UndoCommand_CanExecute));
        }

        public static DependencyProperty ColorProperty;

        public Color Color
        {
            get { return (Color)GetValue(ColorProperty); }
            set { SetValue(ColorProperty, value); }
        }

        public static DependencyProperty RedProperty;
        public static DependencyProperty GreenProperty;
        public static DependencyProperty BlueProperty;

        public byte Red
        {
            get { return (byte)GetValue(RedProperty); }
            set { SetValue(RedProperty, value); }
        }

        public byte Green
        {
            get { return (byte)GetValue(GreenProperty); }
            set { SetValue(GreenProperty, value); }
        }

        public byte Blue
        {
            get { return (byte)GetValue(BlueProperty); }
            set { SetValue(BlueProperty, value); }
        }

        private static void OnColorChanged(DependencyObject sender, DependencyPropertyChangedEventArgs args)
        {
            MyColorPicker cp = (MyColorPicker)sender;
            Color newColor = (Color)args.NewValue;
            Color oldColor = (Color)args.OldValue;
            //args.NewValue;
            cp.Red = newColor.R;
            cp.Green = newColor.G;
            cp.Blue = newColor.B;

            cp.previousColor = oldColor;

            cp.OnColorChanged(oldColor, newColor);
        }

        private static void OnColorRGBChanged(DependencyObject sender, DependencyPropertyChangedEventArgs args)
        {
            MyColorPicker cp = (MyColorPicker)sender;
            Color clr = cp.Color;

            if (args.Property == RedProperty)
                clr.R = (byte)cp.Red;

            if (args.Property == GreenProperty)
                clr.G = (byte)cp.Green;

            if (args.Property == BlueProperty)
                clr.B = (byte)cp.Blue;

            cp.Color = clr;
        }

        public static RoutedEvent ColorChangedEvent = 
            EventManager.RegisterRoutedEvent("ColorChanged", RoutingStrategy.Bubble, 
            typeof(RoutedPropertyChangedEventHandler<Color>), typeof(MyColorPicker));
       

        public event RoutedPropertyChangedEventHandler<Color> ColorChanged
        {
            add { AddHandler(ColorChangedEvent, value); }
            remove { RemoveHandler(ColorChangedEvent, value); }
        }

      

        private void OnColorChanged(Color oldColor, Color newColor)
        {

            RoutedPropertyChangedEventArgs<Color> routedEventArgs = new RoutedPropertyChangedEventArgs<Color>(oldColor, newColor);
            routedEventArgs.RoutedEvent = MyColorPicker.ColorChangedEvent;
            RaiseEvent(routedEventArgs);
        }
    }
}
