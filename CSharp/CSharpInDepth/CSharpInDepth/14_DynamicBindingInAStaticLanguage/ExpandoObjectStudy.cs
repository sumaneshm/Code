using CSharpInDepth.Common;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace CSharpInDepth._14_DynamicBindingInAStaticLanguage
{
    class ExpandoObjectStudy : Study
    {
        public override string StudyName
        {
            get { return "Expando object study"; }
        }

        string xml =
            "<students>" +
            "<student><name>Sumanesh</name><age>33</age></student>" +
            "<student><name>Saveetha</name><age>30</age></student>" +
            "<student><name>Aadhavan</name><age>3</age></student>" +
            "</students>";

        protected override void PerformStudy()
        {
            //SimpleExpando();
            ExpandoXmlDom();

        }

        private dynamic CreateDynamicDoc(XElement element)
        {
            dynamic expando = new ExpandoObject();
            expando.XElement = element;
            expando.ToXml = (Func<string>)element.ToString;

            IDictionary<string, object> dictExpando = expando;

            foreach(XElement child in element.Elements())
            {
                string childName = child.Name.LocalName;
                string childList = childName + "List";
                dynamic childElement = CreateDynamicDoc(child);

                if(!dictExpando.ContainsKey(childName))
                {
                    dictExpando[childName] = childElement;
                    dictExpando[childList] = new List<dynamic> { childElement };
                }
                else
                {
                    ((List<dynamic>)dictExpando[childList]).Add(childElement);
                }
            }

            return expando ;
        }

        private void ExpandoXmlDom()
        {
            XElement xe = XElement.Parse(xml);
            dynamic elem = CreateDynamicDoc(xe);
            Console.WriteLine(elem.studentList[1].name.XElement.Value);
            foreach(dynamic oneStudent in elem.studentList)
            {
                Console.WriteLine("Name : "  + oneStudent.name.ToXml());
                Console.WriteLine("Age : " + oneStudent.age.XElement.Value);
            }
        }

        private void SimpleExpando()
        {
            dynamic exp = new ExpandoObject();
            exp.Name = "Sumanesh";
            exp.PrintName = (Action<string>)(n => Console.WriteLine("Magic : " + n));
            Console.WriteLine(exp.Name);
            exp.PrintName("Welcome");
        }
    }
}
