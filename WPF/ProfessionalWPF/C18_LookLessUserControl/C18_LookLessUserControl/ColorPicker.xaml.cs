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
using System.Windows.Controls.Primitives;

namespace C18_LookLessUserControl
{
    /// <summary>
    /// Interaction logic for ColorPicker.xaml
    /// </summary>
    public partial class ColorPicker : Control
    {
        static ColorPicker()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ColorPicker),
              new FrameworkPropertyMetadata(typeof(ColorPicker)));

            ColorProperty = DependencyProperty.Register("Color", typeof(Color), typeof(ColorPicker), new PropertyMetadata(new PropertyChangedCallback(OnColorChanged)));
            RedProperty = DependencyProperty.Register("Red", typeof(byte), typeof(ColorPicker), new PropertyMetadata(new PropertyChangedCallback(OnColorRGBChanged)));
            GreenProperty = DependencyProperty.Register("Green", typeof(byte), typeof(ColorPicker), new PropertyMetadata(new PropertyChangedCallback(OnColorRGBChanged)));
            BlueProperty = DependencyProperty.Register("Blue", typeof(byte), typeof(ColorPicker), new PropertyMetadata(new PropertyChangedCallback(OnColorRGBChanged)));
        }

        public ColorPicker()
        {
            CommandManager.RegisterClassCommandBinding(typeof(ColorPicker),
              new CommandBinding(ApplicationCommands.Undo, UndoCommand_ExecuteHandler, UndoCommand_CanExecuteHandler));
        }

        private Color? previousColor;

        private void UndoCommand_ExecuteHandler(object sender, ExecutedRoutedEventArgs e)
        {
            ColorPicker cp = (ColorPicker)sender;
            cp.Color = (Color)cp.previousColor;
        }

        private void UndoCommand_CanExecuteHandler(object sender, CanExecuteRoutedEventArgs e)
        {
            ColorPicker cp = (ColorPicker)sender;
            e.CanExecute = cp.previousColor.HasValue;
        }


        private static DependencyProperty ColorProperty;
        private static DependencyProperty RedProperty;
        private static DependencyProperty GreenProperty;
        private static DependencyProperty BlueProperty;

        public Color Color
        {
            get { return (Color)GetValue(ColorProperty); }
            set { SetValue(ColorProperty, value); }
        }

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

        private static void OnColorChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            ColorPicker picker = (ColorPicker)sender;
            Color oldColor = (Color)e.OldValue;
            Color newColor = (Color)e.NewValue;

            picker.Red = newColor.R;
            picker.Green = newColor.G;
            picker.Blue = newColor.B;

            picker.previousColor = oldColor;

            //picker.RaiseEvent(new RoutedPropertyChangedEventArgs<Color>(oldColor, newColor) { RoutedEvent = ColorChangedEvent });
            picker.OnColorChanged(oldColor, newColor);
        }

        private static void OnColorRGBChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            ColorPicker picker = (ColorPicker)sender;
            Color clr = picker.Color;

            if (e.Property == RedProperty)
                clr.R = (byte)e.NewValue;

            if (e.Property == BlueProperty)
                clr.B = (byte)e.NewValue;

            if (e.Property == GreenProperty)
                clr.G = (byte)e.NewValue;

            picker.Color = clr;
        }

        private static RoutedEvent ColorChangedEvent =
            EventManager.RegisterRoutedEvent("ColorChanged",
                RoutingStrategy.Bubble,
                typeof(RoutedPropertyChangedEventHandler<Color>), typeof(ColorPicker));

        public event RoutedPropertyChangedEventHandler<Color> ColorChanged
        {
            add
            {
                AddHandler(ColorChangedEvent, value);
            }
            remove
            {
                RemoveHandler(ColorChangedEvent, value);
            }
        }

        private void OnColorChanged(Color oldValue, Color newValue)
        {
            RoutedPropertyChangedEventArgs<Color> args = new RoutedPropertyChangedEventArgs<Color>(oldValue, newValue);
            args.RoutedEvent = ColorPicker.ColorChangedEvent;
            RaiseEvent(args);
        }


    

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            RangeBase redSlider = GetTemplateChild("PART_RedSlider") as RangeBase;
            if (redSlider != null)
            {
                Binding binding = new Binding("Red");
                binding.Source = this;
                binding.Mode = BindingMode.TwoWay;
                redSlider.SetBinding(RangeBase.ValueProperty, binding);
            }

            RangeBase greenSlider = GetTemplateChild("PART_GreenSlider") as RangeBase;
            if (greenSlider != null)
            {
                Binding binding = new Binding("Green");
                binding.Source = this;
                binding.Mode = BindingMode.TwoWay;
                greenSlider.SetBinding(RangeBase.ValueProperty, binding);
            }

            RangeBase blueSlider = GetTemplateChild("PART_BlueSlider") as RangeBase;
            if (blueSlider != null)
            {
                Binding binding = new Binding("Blue");
                binding.Source = this;
                binding.Mode = BindingMode.TwoWay;
                blueSlider.SetBinding(RangeBase.ValueProperty, binding);
            }

            SolidColorBrush brush = GetTemplateChild("PART_PreviewBrush") as SolidColorBrush;
            if (brush != null)
            {
                Binding binding = new Binding("Color");
                binding.Source = brush;
                binding.Mode = BindingMode.OneWayToSource;
                this.SetBinding(ColorPicker.ColorProperty, binding);
            }

        }
    }
}
