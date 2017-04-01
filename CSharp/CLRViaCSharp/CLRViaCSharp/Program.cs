using System.Collections;
using System.Xml.Linq;
using System.Xml.XPath;
using CLRViaCSharp.Common;
using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Castle.Windsor;

namespace CLRViaCSharp
{
    /* // Study 1 
    class Person
    {
        public string Name { get; set; }

        public void PrintName()
        {
            Console.WriteLine("Welcome Mr. " + Name);
        }
    }
   
    class BaseClass
    {
        public virtual void PrintMe()
        {
            Console.WriteLine("Base : PrintMe");
        }
    }

    class Derived : BaseClass
    {
        public override void PrintMe()
        {
            Console.WriteLine("Derived : PrintMe");
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
           

            //Person p = new Person { Name = "Sumanesh" };
           

            //Console.WriteLine(p.Name);

            //BaseClass b = new Derived();
            //b.PrintMe();


            //Console.WriteLine(b.GetType());
        }
    }
     */

    // Explores operator overloading.
    //class Student
    //{
    //    public static int TotalCount { get; set; }

    //    public int Mark {get; set;}

    //    public static Student operator +(Student s1, Student s2)
    //    {
    //        return new Student{Mark= s1.Mark+s2.Mark};
    //    }
    //}

    //class ChildStudent : Student
    //{
    //    public int Mark { get; set; }
    //}

    //class Program
    //{
    //    static void Main(string[] args)
    //    {
    //        Int32 s = 10;
    //        Console.WriteLine(s+5);
    //    }
    //}

    // Explores the optional parameters
    //class Student
    //{
    //    public void MarkIt(int science, int maths = 20)
    //    {

    //    }
    //}

    //class Program
    //{
    //    static void Main(string[] args)
    //    {
    //        Student s = new Student();
    //        s.MarkIt(10);
    //    }
    //}




    class Program
    {

        static void Main(string[] args)
        {
            //WindsorContainer container = CastleHelper.Container;

            //Study study = container.Resolve<Study>();

            //study.Exihibit();

            string xml = "<?xml version='1.0'?><Students><Student><Name age='4'>Aadhavan</Name><Class>Tulip</Class></Student><Student><Name age='1'>Nila</Name><Class>Lotus</Class></Student></Students>";
            XDocument doc = XDocument.Parse(xml);
            var element = ((IEnumerable) doc.XPathEvaluate("Students/Student[2]/Name/@age")).OfType<XElement>();
            if (element != null && element.Any())
            {
                Console.WriteLine("Elements");
                foreach (var ele in element)
                {
                    
                    Console.WriteLine(element == null ? "Empty" : ele.Value);
                }
            }

            var attributes = ((IEnumerable)doc.XPathEvaluate("Students/Student[1]/Name/@age")).OfType<XAttribute>();
            if (attributes != null && attributes.Any())
            {
                Console.WriteLine("Attributes");
                foreach (var ele in attributes)
                {

                    Console.WriteLine(element == null ? "Empty" : ele.Value);
                }
            }
            

        }

    }
}
