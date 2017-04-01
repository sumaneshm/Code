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
    //This DynamicXElement is better than ExpandoObjectDom we saw in ExpandoObjectStudy
    // 1. It doesn't require us to navigate through entire tree in the beginning
    // 2. It is not stale, i.e. will reflect any changes in the underlying data

    public class DynamicXElement : DynamicObject
    {
        private readonly XElement element;

        private DynamicXElement(XElement element)
        {
            this.element = element;
        }

        public static DynamicXElement CreateInstance(XElement element)
        {
            return new DynamicXElement(element);
        }

        public override string ToString()
        {
            return element.Value;
        }

        public XElement XElement
        {
            get { return element; }
        }

        // DLR will first check whether the object explicitly implements member which is referenced before calling any of the TryXXX() methods
        // hence when any user of the class DynamicXElement calls indexer either using a string or integer, it will directly call the below mentioned 
        // indexer property before trying TryGetIndex 
        public XAttribute this[string name]
        {
            get
            {
                return element.Attribute(name);
            }
        }

        public dynamic this[int index]
        {
            get
            {
                var parent = element.Parent;
                if (parent == null)
                {
                    if (index != 0)
                    {
                        throw new ArgumentOutOfRangeException();
                    }
                    return this;
                }
                XElement ele = parent.Elements(element.Name).ElementAt(index);
                return ele != null ? new DynamicXElement(ele) : this;
            }
        }


        // When any property is called on the object, DLR will try to call this method to check whether the passed element 
        // is supported and if it is, then will return the appropriate otherwise will call the baseclass
        
        // TIP!!! Always call the baseclass.TryXXX method when the current class doesnt support the element which it is looking for
        // as it would require two statements (set result = null and return false) if it does it on its own

        public override bool TryGetMember(GetMemberBinder binder, out object result)
        {
            var childName = binder.Name;
            var child = element.Element(childName);
            if(child != null)
            {
                result = new DynamicXElement(child);
                return true;
            }

            return base.TryGetMember(binder, out result);
        }

        // Very handy for Debugging cases where it will display the elements and its children
        public override IEnumerable<string> GetDynamicMemberNames()
        {
            return element.Elements()
                        .Select(e => e.Name.LocalName)
                        .Distinct()
                        .OrderBy(x => x);
        }
    }

    class DynamicObjectDom : Study
    {

        string xml =
          "<students>" +
          "<student><name initial='M'>Sumanesh</name><age>33</age></student>" +
          "<student><name>Saveetha</name><age>30</age></student>" +
          "<student><name>Aadhavan</name><age>3</age></student>" +
          "</students>";


        public override string StudyName
        {
            get { return "DynamicObject implementation"; }
        }

        protected override void PerformStudy()
        {
            XElement element = XElement.Parse(xml);
            dynamic xele = DynamicXElement.CreateInstance(element);

            Console.WriteLine(xele.student.name["initial"]);
        }
    }
}