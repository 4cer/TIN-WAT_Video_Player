using SerwisTIN;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Description;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;

namespace SerwisHostTIN
{
    class StreamerHost
    {
        static void Main(string[] args)
        {
            using (ServiceHost host = new ServiceHost(typeof(SerwisTIN.StreamerService)))
            {
                host.Open();
                Console.WriteLine("Service is live now at: {0}", "http://127.0.0.1:8080/StreamerService");
                Console.WriteLine("Press Spacebar to exit!");
                while(Console.ReadKey().Key != ConsoleKey.Spacebar)
                {

                }
            }
            //ServiceHost streamerServiceHost = null;
            //try
            //{
            //    //Base Address for StreamerService
            //    Uri httpBaseAddress = new Uri("http://localhost:8081/StreamerService");

            //    //Instantiate ServiceHost
            //    streamerServiceHost = new ServiceHost(typeof(SerwisTIN.StreamerService), httpBaseAddress);

            //    //Add Endpoint to Host
            //    streamerServiceHost.AddServiceEndpoint(typeof(SerwisTIN.IStreamerService), new WSHttpBinding(), "");

            //    //Metadata Exchange
            //    ServiceMetadataBehavior serviceBehavior = new ServiceMetadataBehavior();
            //    serviceBehavior.HttpGetEnabled = true;
            //    streamerServiceHost.Description.Behaviors.Add(serviceBehavior);

            //    //Open
            //    streamerServiceHost.Open();
            //    Console.WriteLine("Service is live now at: {0}", httpBaseAddress);
            //    Console.ReadKey();
            //}
            //catch (Exception ex)
            //{
            //    streamerServiceHost = null;
            //    Console.WriteLine("There is an issue with StreamerService\n" +ex.Message);
            //}
        }
    }
}
