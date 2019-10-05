using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace TestClient
{
    class Program
    {
        public static int Main(String[] args)
        {
            Console.WriteLine("Please enter the IP Address of the server");
            Client client = new Client(Console.ReadLine());
            string message = "{\"route\": \"newPlayer\", \"data\": {\"name\": \"Hubertus\"}}";
            client.StartClient(message);
            return 0;
        }

    }
}
