using CommonLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client
{
    class Program
    {
        static void Main(string[] args)
        {
            using (DataService.WCFDataServiceClient client = new DataService.WCFDataServiceClient("BasicHttpBinding_IWCFDataService"))
            {
                Console.WriteLine("Sending an Employee to edit");
                client.Edit(new Employee { Name = "Aadhavan", RollNumber = 12 });

                Console.WriteLine("Sending an Student to add");
                //client.Add(new Student{StudentName="Sumanesh",Class="BSc"});
            }
            Console.WriteLine("Element sent successfully");
            Console.ReadLine();
        }
    }
}
