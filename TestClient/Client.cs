using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace TestClient
{
    class Client
    {
        public string Eof = "!<tlbzXnAz5mYvMJoC5uUJ*tlbzXnAz5mYvMJoC5uUJ>!";
        public IPAddress IPAddress { get; set; }

        public Client(String ip)
        {
            this.IPAddress = IPAddress.Parse(ip);
        }

        public void StartClient(string message)
        {
            // Data buffer for incoming data.  
            byte[] bytes = new byte[1024];

            // Connect to a remote device.  
            try
            {
                // Establish the remote endpoint for the socket.  
                // This example uses port 11000 on the local computer.  
                IPEndPoint remoteEP = new IPEndPoint(IPAddress, 23312);

                // Create a TCP/IP  socket.  
                using (Socket sender = new Socket(AddressFamily.InterNetwork,
                    SocketType.Stream, ProtocolType.Tcp))
                {

                    // Connect the socket to the remote endpoint. Catch any errors.  
                    try
                    {
                        sender.Connect(remoteEP);

                        Console.WriteLine("[CONNECTION] {0}", sender.RemoteEndPoint.ToString());
                        // Encode the data string into a byte array.  
                        byte[] msg = Encoding.ASCII.GetBytes(message + Eof);

                        // Send the data through the socket.  
                        int bytesSent = sender.Send(msg);

                        // Receive the response from the remote device.  
                        int bytesRec = sender.Receive(bytes);
                        string Data = Encoding.ASCII.GetString(bytes, 0, bytesRec);
                        Data = Data.Replace(Eof, "");
                        Console.WriteLine("[RESPONSE] {0}", Data);

                        // Release the socket.  
                        sender.Shutdown(SocketShutdown.Both);
                        sender.Close();

                    }
                    catch (ArgumentNullException ane)
                    {
                        Console.WriteLine("ArgumentNullException : {0}", ane.ToString());
                    }
                    catch (SocketException se)
                    {
                        Console.WriteLine("SocketException : {0}", se.ToString());
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine("Unexpected exception : {0}", e.ToString());
                    }
                }

            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }
    }
}
