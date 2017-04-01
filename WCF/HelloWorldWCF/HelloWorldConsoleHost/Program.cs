using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Description;
using System.Text;

namespace HelloWorldConsoleHost
{
    class Program
    {
        static void Main(string[] args)
        {
            Uri baseAddress = new Uri("net.tcp://localhost:8523/Service1");

            using (ServiceHost host = new ServiceHost(typeof(HelloWorldWCF.Service1), baseAddress ))
            {
                ServiceMetadataBehavior smb = new ServiceMetadataBehavior();
                smb.HttpGetEnabled = false;
                smb.MetadataExporter.PolicyVersion = PolicyVersion.Policy15;
                host.Description.Behaviors.Add(smb);

                host.Open();

                Console.WriteLine("The service is ready at: {0}", baseAddress);
                Console.WriteLine("Press <Enter> to stop the service");
                Console.ReadKey();
                host.Close();

            }
        }
    }
}
