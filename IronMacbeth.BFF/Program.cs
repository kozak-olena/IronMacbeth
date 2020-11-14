using System;
using System.IdentityModel.Configuration;
using System.ServiceModel;

namespace IronMacbeth.BFF
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            using (var host = new ServiceHost(typeof (Service)))          
            {
                host.Credentials.UseIdentityConfiguration = true;

                var idConfig = new IdentityConfiguration();

                idConfig.SecurityTokenHandlers.AddOrReplace(new UserNameSecurityTokenHandler());

                host.Credentials.IdentityConfiguration = idConfig;

                Console.WriteLine("Starting Server...");

                try
                {
                    host.Open();
                }
                catch (Exception exception)
                {
                    Console.WriteLine("Unable to start server");
                    Console.WriteLine("Error: ");
                    Console.WriteLine(exception.Message);
                    Console.WriteLine("Press any key to continue");
                    Console.ReadKey();
                    host.Abort();
                    return;
                }
                Console.WriteLine("Server is running");
                Console.WriteLine("Type \"exit\" to  stop server");
                while (Console.ReadLine() != "exit") { }
            }
        }
    }
}
