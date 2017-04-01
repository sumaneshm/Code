using CSharpInDepth.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharpInDepth._13_MinorChangesToSimplyCode
{
    class VarianceRevisited : Study
    {
        /*
         * Covariance - Out/ Derived assigned to Base
         * Contravariance - In / Base assigned to Derived
         */

        public override string StudyName
        {
            get { return "Revisiting both Covariance and contravariance"; }
        }

        public class BaseClass { }

        public class DerivedClass : BaseClass { }

        interface IContra<in T>
        {
            void AddMe(T itemToAdd);
        }

        interface ICovariance<out T>
        {
            T SendOut();
        }

        class ContraClass<T> : IContra<T>, ICovariance<T>
        {
            private List<T> list = new List<T>();

            public void AddMe(T itemToAdd)
            {
                list.Add(itemToAdd);
            }

            public T SendOut()
            {
                return list.FirstOrDefault();
            }
        }

        protected override void PerformStudy()
        {
            InterfaceVariance();
            DelegateVariance();

            SimultaneousVariance();

        }

        private static void SimultaneousVariance()
        {
            //Converter<in TInput, out TOutput>

            Converter<object, string> objStrConverter = o => o.ToString();
            Converter<string, string> strStrConverter = objStrConverter; // Contravariance 
            Converter<object, object> objObjConverter = objStrConverter; // Covariance
            Converter<string, object> strObjConverter = objStrConverter; // Exihits both type of variance

        }

        private static void DelegateVariance()
        {
            // Declaration : Action<in T>
            // Example : Contravariance
            // When we are expecting that a object will passed, we can definitely pass in a String

            Action<string> stringAction = (s => Console.WriteLine(s.Length));
            Action<object> objectAction = (o => Console.WriteLine("This is an objectAction " + o.ToString()));

            stringAction = objectAction;

            stringAction("10");

            // Declaration : Func<out T> 
            // Example : Covariance
            // When we are expecting a object as return type, we can definitely return string back

            Func<String> stringFunc = () => "Sumanesh";
            Func<object> objectFunc = () => new object();

            objectFunc = stringFunc;

            Console.WriteLine(objectFunc());
        }

        private static void InterfaceVariance()
        {
            ContraClass<BaseClass> baseClass = new ContraClass<BaseClass>();
            ContraClass<DerivedClass> derivedClass = new ContraClass<DerivedClass>();

            //Interface - Contravariance
            // Item going in.....
            // When we are expecting a BaseClass, we can pass a DerivedClass "IN"

            IContra<DerivedClass> id = baseClass;
            id.AddMe(new DerivedClass());

            // Interface - Covariance
            // Item coming out....
            // When we are expecting that a BaseClass is going to come out, we can definitely pass a DerivedClass out of it...
            ICovariance<BaseClass> ib = derivedClass;
            Console.WriteLine(ib.SendOut());
        }
    }

}
