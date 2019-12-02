using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace Blackjack
{
    class Program
    {
        static void Main(string[] args)
        {
            //Console.WriteLine("Please enter the IPv4 Address of this server");
            //Server server = new Server(Console.ReadLine());

            //Console.WriteLine("Starting Listener on {0}:{1}", server.IpAddress, server.Port);

            //server.StartListener();

            Game.Game game = new Game.Game();

            game.AddPlayer(new Player("Kai"));
            game.AddPlayer(new Player("Max"));

            game.StartGame();
        }
    }
}
