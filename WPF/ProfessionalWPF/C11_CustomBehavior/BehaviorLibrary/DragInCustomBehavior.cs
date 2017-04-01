using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Interactivity;
using System.Windows.Controls;
using System.Windows.Media;

namespace BehaviorLibrary
{
    public class DragInCustomBehavior : Behavior<UIElement>
    {
        private Canvas canvas;
        private bool isDragging = false;
        private Point mouseOffset;


        protected override void OnAttached()
        {
            base.OnAttached();
            this.AssociatedObject.MouseDown += AssociatedObject_MouseDown;
            this.AssociatedObject.MouseMove += AssociatedObject_MouseMove;
            this.AssociatedObject.MouseUp += AssociatedObject_MouseUp;
        }

        protected override void OnDetaching()
        {
            base.OnDetaching();

            this.AssociatedObject.MouseDown -= AssociatedObject_MouseDown;
            this.AssociatedObject.MouseMove -= AssociatedObject_MouseMove;
            this.AssociatedObject.MouseUp -= AssociatedObject_MouseUp;
        }

        void AssociatedObject_MouseUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (isDragging)
            {
                this.AssociatedObject.ReleaseMouseCapture();
                this.AssociatedObject.Opacity = 1;
                isDragging = false;
            }
        }

        void AssociatedObject_MouseMove(object sender, System.Windows.Input.MouseEventArgs e)
        {
            if (isDragging)
            {
                Point point = e.GetPosition(canvas);
                this.AssociatedObject.SetValue(Canvas.TopProperty, point.Y - mouseOffset.Y);
                this.AssociatedObject.SetValue(Canvas.LeftProperty, point.X - mouseOffset.X);
            }
        }

        void AssociatedObject_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (canvas == null)
                canvas = (Canvas)VisualTreeHelper.GetParent(this.AssociatedObject);
            isDragging = true;
            mouseOffset = e.GetPosition(this.AssociatedObject);
            this.AssociatedObject.Opacity = 0.3;
            this.AssociatedObject.CaptureMouse();
        }
    }
}
