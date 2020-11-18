using IronMacbeth.FileManagement;
using System;
using System.ServiceModel;


namespace FileService
{
    class Program
    {
        static void Main(string[] args)
        {
            using (ServiceHost host = new ServiceHost(typeof(FileManagementService)))
            {
                host.Open();
                Console.WriteLine("The service is ready at {0}");
                Console.WriteLine("Press <Enter> to stop the service.");
                Console.ReadLine();
                host.Close();
            }
        }
    }
}

