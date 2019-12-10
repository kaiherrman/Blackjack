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
            Client client = new Client("10.10.136.228");

            while (true)
            {
                Console.WriteLine("Enter JSON: ");
                client.StartClient(Console.ReadLine());
            }
        }

    }
}
