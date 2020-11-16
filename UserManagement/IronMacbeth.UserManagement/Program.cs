using System;
using System.ServiceModel;

namespace IronMacbeth.UserManagement
{
    class Program
    {
        static void Main(string[] args)
        {
            using (ServiceHost host = new ServiceHost(typeof(UserManagementService)))
            {
                host.Open();

                Console.WriteLine("Press <Enter> to stop the service.");
                Console.ReadLine();

                host.Close();
            }
        }
    }
}
