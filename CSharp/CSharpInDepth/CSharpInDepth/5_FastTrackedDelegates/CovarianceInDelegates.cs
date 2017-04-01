using CSharpInDepth.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace CSharpInDepth._5_FastTrackedDelegates
{
    //Listing 5.3 
    
    class CovarianceInDelegates : Study
    {
        delegate Stream StreamFactory();

        static MemoryStream GetMemoryStream()
        {
            byte[] bytes = new byte[10];

            for(int i=0;i<bytes.Length;i++)
            {
                bytes[i] = (byte) (i + 1);
            }

            return new MemoryStream(bytes);
        }

        public override string StudyName
        {
            get { return "Covariance in Delegates"; }
        }

        protected override void PerformStudy()
        {
            StreamFactory sf = new StreamFactory(GetMemoryStream); // Delegate Covariance - applied here   

            using(Stream mems = sf())
            {
                int data;
                while((data=mems.ReadByte()) != -1)
                {
                    Console.WriteLine(data);
                }
            }
        }
    }
}
