using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using System.Windows;
using System.Windows.Controls.Primitives;

namespace C18_FlipPanelControl
{
    [TemplateVisualState(Name="Normal",GroupName="FlipStatus")]
    [TemplateVisualState(Name="Flipped",GroupName="FlipStatus")]
    [TemplatePart(Name="FlipButton",Type=typeof(FlipPanel))]
    [TemplatePart(Name="FlipButtonAlternate",Type=typeof(FlipPanel))]
    public class FlipPanel : Control
    {
        public FlipPanel()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(FlipPanel), new FrameworkPropertyMetadata(typeof(FlipPanel)));
        }

        public static readonly DependencyProperty FrontContentProperty = DependencyProperty.Register("FrontContent", typeof(object), typeof(FlipPanel), null);

        public object FrontContent
        {
            set
            {
                SetValue(FrontContentProperty, value);
            }

            get
            {
                return GetValue(FrontContentProperty);
            }
        }


        public static readonly DependencyProperty BackContentProperty = DependencyProperty.Register("BackContent", typeof(object), typeof(FlipPanel), null);

        public object BackContent
        {
            set { SetValue(BackContentProperty, value); }
            get { return GetValue(BackContentProperty); }

        }

        public bool IsFlipped
        {
            get { return (bool)GetValue(IsFlippedProperty); }
            set { SetValue(IsFlippedProperty, value); }
        }

        // Using a DependencyProperty as the backing store for IsFlippedProperty.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IsFlippedProperty =
            DependencyProperty.Register("IsFlipped", typeof(bool), typeof(FlipPanel),new PropertyMetadata(new PropertyChangedCallback(IsFlippedChanged)));

        private static void IsFlippedChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            FlipPanel panel = (FlipPanel)sender;
            panel.ChangeVisualState((bool)e.NewValue);

        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            ToggleButton flipButton = base.GetTemplateChild("FlipButton") as ToggleButton;
            if(flipButton != null)
                flipButton.Click += flipButton_Click;
            ToggleButton flipButtonAlternate = base.GetTemplateChild("FlipButtonAlternate") as ToggleButton;

            if (flipButtonAlternate != null)
                flipButtonAlternate.Click += flipButton_Click;

            this.ChangeVisualState(false);
        }

        void flipButton_Click(object sender, RoutedEventArgs e)
        {
            this.IsFlipped = !this.IsFlipped;
            ChangeVisualState(true);
        }


        public CornerRadius CornerRadius
        {
            get { return (CornerRadius)GetValue(CornerRadiusProperty); }
            set { SetValue(CornerRadiusProperty, value); }
        }

        // Using a DependencyProperty as the backing store for CornerRadiusProperty.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CornerRadiusProperty =
            DependencyProperty.Register("CornerRadius", typeof(CornerRadius), typeof(FlipPanel), null);



        private void ChangeVisualState(bool useTransitions)
        {
            if (IsFlipped)
            {
                VisualStateManager.GoToState(this, "Flipped", useTransitions);
            }
            else
            {
                VisualStateManager.GoToState(this, "Normal", useTransitions);
            }
        }

    }
}
