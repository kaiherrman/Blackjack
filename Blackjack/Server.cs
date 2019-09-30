using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Blackjack
{
    class Server
    {

        public string Eof = "!<tlbzXnAz5mYvMJoC5uUJ*tlbzXnAz5mYvMJoC5uUJ>!";
        public string Data = null;
        public IPAddress IpAddress { get; set; }
        public int Port { get; set; }

        public Server(IPAddress ip, int port)
        {
            this.IpAddress = ip;
            this.Port = port;
        }

        public Server(string ipString, int port)
        {
            this.Port = port;
            try
            {
                IpAddress = IPAddress.Parse(ipString);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }

        public Server(IPAddress ip)
        {
            this.IpAddress = ip;
            this.Port = 23312;
        }

        public Server(string ipString)
        {
            this.Port = 23312;
            try
            {
                IpAddress = IPAddress.Parse(ipString);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }

        public void StartListener()
        {
            byte[] bytes = new Byte[1024];
            Socket listener = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            // Bind the socket to the local endpoint and   
            // listen for incoming connections.  
            try
            {
                listener.Bind(new IPEndPoint(IpAddress, 23312));
                listener.Listen(10);

                // Start listening for connections.  
                while (true)
                {
                    Console.WriteLine("Waiting for a connection...");
                    // Program is suspended while waiting for an incoming connection.  
                    Socket handler = listener.Accept();
                    Data = null;

                    // An incoming connection needs to be processed.  
                    while (true)
                    {
                        int bytesRec = handler.Receive(bytes);
                        Data += Encoding.ASCII.GetString(bytes, 0, bytesRec);
                        if (Data.IndexOf(Eof) > -1)
                        {
                            break;
                        }
                    }

                    Data = Data.Replace(Eof, "");
                    // Show the data on the console.  
                    Console.WriteLine("Received: {0}", Data);

                    JObject jObject = JObject.Parse(Data);
                    string route = (string)jObject.SelectToken("route");
                    JObject data = (JObject)jObject.SelectToken("data");

                    Router.Route(route, data);

                    // Echo the data back to the client.  
                    byte[] msg = Encoding.ASCII.GetBytes(Data);

                    handler.Send(msg);
                    handler.Shutdown(SocketShutdown.Both);
                    handler.Close();
                }

            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }

            Console.WriteLine("\nPress ENTER to continue...");
            Console.Read();
        }
    }
}
