using System;
using System.Collections.Generic;
using CSharpInDepth.Common;

namespace CSharpInDepth.Chapter3
{
    //Listing 3.14

    //Explains how to handle Generics contra variance.
    // -> makes use of interface/classes declared in covariance example


    class AreaComparer : IComparer<IShape>
    {
        public int Compare(IShape x, IShape y)
        {
            return x.Area < y.Area ? 1 : 0;
        }
    }

    class GenericsContraVariance : Study
    {

        public override string StudyName
        {
            get { return "Contra variance in Generics"; }
        }

        protected override void PerformStudy()
        {
            IComparer<IShape> comparer = new AreaComparer();

            List<Circle> circles = new List<Circle>
            {
                new Circle {Area = 120},
                new Circle {Area = 20},
                new Circle {Area = 130},
                new Circle {Area = 440}
            };

            circles.Sort(comparer);

            foreach (var c in circles)
            {
                Console.WriteLine(c.Area);
            }
        }
    }
}
