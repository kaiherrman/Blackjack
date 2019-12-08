using Blackjack_v2.bj;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Blackjack_v2.SocketComm
{
    class Server
    {
        public string Eof = "!<tlbzXnAz5mYvMJoC5uUJ*tlbzXnAz5mYvMJoC5uUJ>!";
        public string Data = null;
        public IPAddress IpAddress { get; set; }
        public int Port { get; set; }
        public Router Router { get; set; }
        public Game Game { get; set; }

        public Server(string ipString,Game game, int port = 23312)
        {
            Game = game;
            Router = new Router(this);
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

        public void StartListener()
        {
            byte[] bytes = new Byte[1024];
            Socket listener = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            // Bind the socket to the local endpoint and   
            // listen for incoming connections.  
            try
            {
                listener.Bind(new IPEndPoint(IpAddress, Port));
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
                    JObject response = this.Router.Route(jObject);

                    // Echo the data back to the client.  
                    byte[] msg = Encoding.ASCII.GetBytes(response.ToString() + Eof);

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
