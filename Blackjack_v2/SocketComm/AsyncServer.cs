﻿using System;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using Newtonsoft.Json.Linq;
using Blackjack_Server.bj;

namespace Blackjack_Server.SocketComm
{
    // State object for reading client data asynchronously  
    public class StateObject
    {
        // Client  socket.  
        public Socket WorkSocket;
        // Size of receive buffer.  
        public const int BufferSize = 1024;
        // Receive buffer.  
        public readonly byte[] Buffer = new byte[BufferSize];
        // Received data string.  
        public readonly StringBuilder Sb = new StringBuilder();
    }

    class AsyncServer
    {
        public AsyncServer(string ipString, Game game, int port = 23312)
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

        // Thread signal.  
        private static readonly ManualResetEvent AllDone = new ManualResetEvent(false);

        private IPAddress IpAddress { get; }
        private int Port { get; }
        private Router Router { get; }
        public Game Game { get; set; }

        public void StartListening()
        {
            // Establish the local endpoint for the socket.  
            IPEndPoint localEndPoint = new IPEndPoint(IpAddress, Port);

            // Create a TCP/IP socket.  
            Socket listener = new Socket(IpAddress.AddressFamily,
                SocketType.Stream, ProtocolType.Tcp);

            // Bind the socket to the local endpoint and listen for incoming connections.  
            try
            {
                listener.Bind(localEndPoint);
                listener.Listen(100);

                while (true)
                {
                    // Set the event to non signaled state.  
                    AllDone.Reset();

                    // Start an asynchronous socket to listen for connections.  
                    Console.WriteLine("Waiting for a connection...");
                    listener.BeginAccept(
                        AcceptCallback,
                        listener);

                    // Wait until a connection is made before continuing.  
                    AllDone.WaitOne();
                }

            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }

            Console.WriteLine("\nPress ENTER to continue...");
            Console.Read();

        }

        private void AcceptCallback(IAsyncResult ar)
        {
            // Signal the main thread to continue.  
            AllDone.Set();

            // Get the socket that handles the client request.  
            Socket listener = (Socket)ar.AsyncState;
            Socket handler = listener.EndAccept(ar);

            // Create the state object.  
            StateObject state = new StateObject {WorkSocket = handler};
            handler.BeginReceive(state.Buffer, 0, StateObject.BufferSize, 0,
                ReadCallback, state);
        }

        private void ReadCallback(IAsyncResult ar)
        {
            // Retrieve the state object and the handler socket  
            // from the asynchronous state object.  
            StateObject state = (StateObject)ar.AsyncState;
            Socket handler = state.WorkSocket;

            // Read data from the client socket.   
            int bytesRead = handler.EndReceive(ar);

            if (bytesRead > 0)
            {
                // There  might be more data, so store the data received so far.  
                state.Sb.Append(Encoding.ASCII.GetString(
                    state.Buffer, 0, bytesRead));

                // Check for end-of-file tag. If it is not there, read   
                // more data.  
                var content = state.Sb.ToString();
                if (content.IndexOf("<EOF>") > -1)
                {
                    // All the data has been read from the   
                    // client. Display it on the console.  
                    Console.WriteLine("Read {0} bytes from socket. \n Data : {1}",
                        content.Length, content);
                    // Echo the data back to the client.  
                    content = content.Replace("<EOF>", "");
                    JObject jObject = JObject.Parse(content);
                    JObject response = Router.Route(jObject);
                    Send(handler, response + "<EOF>");
                }
                else
                {
                    // Not all data received. Get more.  
                    handler.BeginReceive(state.Buffer, 0, StateObject.BufferSize, 0,
                    ReadCallback, state);
                }
            }
        }

        private void Send(Socket handler, String data)
        {
            // Convert the string data to byte data using ASCII encoding.  
            byte[] byteData = Encoding.ASCII.GetBytes(data);

            // Begin sending the data to the remote device.  
            handler.BeginSend(byteData, 0, byteData.Length, 0,
                SendCallback, handler);
        }

        private void SendCallback(IAsyncResult ar)
        {
            try
            {
                // Retrieve the socket from the state object.  
                Socket handler = (Socket)ar.AsyncState;

                // Complete sending the data to the remote device.  
                int bytesSent = handler.EndSend(ar);
                Console.WriteLine("Sent {0} bytes to client.", bytesSent);

                handler.Shutdown(SocketShutdown.Both);
                handler.Close();

            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }
    }
}
