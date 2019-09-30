using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace Blackjack
{
    class Program
    {
        public static string eof = "!<tlbzXnAz5mYvMJoC5uUJ*tlbzXnAz5mYvMJoC5uUJ>!";
        public static string data = null;

        static void Main(string[] args)
        {
            byte[] bytes = new Byte[1024];
            Socket listener = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            // Bind the socket to the local endpoint and   
            // listen for incoming connections.  
            try
            {
                listener.Bind(new IPEndPoint(IPAddress.Parse("10.10.136.61"), 23312));
                listener.Listen(10);

                // Start listening for connections.  
                while (true)
                {
                    Console.WriteLine("Waiting for a connection...");
                    // Program is suspended while waiting for an incoming connection.  
                    Socket handler = listener.Accept();
                    data = null;

                    // An incoming connection needs to be processed.  
                    while (true)
                    {
                        int bytesRec = handler.Receive(bytes);
                        data += Encoding.ASCII.GetString(bytes, 0, bytesRec);
                        if (data.IndexOf(eof) > -1)
                        {
                            break;
                        }
                    }

                    data = data.Replace(eof, "");
                    // Show the data on the console.  
                    Console.WriteLine("Text received : {0}", data);

                    // Echo the data back to the client.  
                    byte[] msg = Encoding.ASCII.GetBytes(data);

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

        //static void Main(string[] args)
        //{
        //    Deck deck = new Deck(); //Create Deck
        //    Dealer dealer = new Dealer(deck); //Create Dealer

        //    List<Player> players = new List<Player>
        //    {
        //        //Create 2 players
        //        new Player("Max"),    
        //        new Player("Dudeson")
        //    };

        //    Console.WriteLine("Dealer Cards:");
        //    Console.WriteLine("--------------------------");
        //    Console.WriteLine(dealer.Hand.Cards[0].Display());
        //    Console.WriteLine("--------------------------");
        //    Console.WriteLine("");


        //    foreach (Player player in players)
        //    {
        //        dealer.DealCards(player);
        //        Console.WriteLine("Player [{0}] Cards:", player.Name);
        //        Console.WriteLine("--------------------------");
        //        foreach (Card card in player.Hand.Cards)
        //        {
        //            Console.WriteLine(card.Display());
        //        }
        //        Console.WriteLine("--------------------------");
        //        Console.WriteLine("");
        //    }
        //}
    }
}
