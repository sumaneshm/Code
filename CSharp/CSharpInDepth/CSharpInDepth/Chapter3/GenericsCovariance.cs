using CSharpInDepth.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharpInDepth.Chapter3
{
    // Figure 3.4 (no listing in the book for this)

    // Explains how to handle Covariance issue with Generics
    // Covariance => trying to convert SomeType<Child> to SomeType<Parent>
    // Contravariance => Trying to convert SomeType<Parent> to SomeType<Child>

    interface IShape {  double Area { get; set; } }

    class Circle : IShape
    {
        public double Area { get; set; }
    }

    class Rectangle : IShape {
        public double Area { get; set; }
    }

    abstract class Drawing
    {
        protected IEnumerable<IShape> Shapes { get; set; }
    }

    class RectDrawing : Drawing
    {
        public List<Rectangle> Rectangles
        {
            get
            {
                //The following line requires Shapes to be declared as List

                //return Shapes.ConvertAll<Rectangle>(s => (Rectangle)s);

                //return Shapes.Cast<Rectangle>().ToList();

                return Shapes.OfType<Rectangle>().ToList();
            }
        }
    }

    class CircDrawing : Drawing
    {
        public List<Circle> Circles
        {
            get
            {
                return Shapes.ToList().ConvertAll<Circle>(s => (Circle)s);
            }
        }
    }

    class GenericsCovariance : Study
    {
        public override string StudyName
        {
            get { return "Explains Covariance implementation for Generics"; }
        }

        protected override void PerformStudy()
        {
            Console.WriteLine("Nothing to output, just go through the code and read comments :) ");
        }
    }
}
