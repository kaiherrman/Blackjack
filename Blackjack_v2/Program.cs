using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Blackjack_v2.bj;
using Blackjack_v2.SocketComm;

namespace Blackjack_v2
{
    class Program
    {
        static void Main(string[] args)
        {
            Game game = new Game();

            string ipAddress = "192.168.1.132"; //Get from user-input or config
            Server server = new Server(ipString: ipAddress, game: game);

            server.StartListener();
        }
        //static void Main(string[] args)
        //{
        //    Game game = new Game();

        //    //Get Player Names
        //    Console.Write("Enter First Player Name: ");
        //    game.AddPlayer(new Player(Console.ReadLine()));
        //    Console.Write("Enter Second Player Name: ");
        //    game.AddPlayer(new Player(Console.ReadLine()));
        //    Console.Clear();

        //    game.NewRound();

        //    Console.Write(Environment.NewLine);

        //    while (game.IsRunning)
        //    {
        //        game.CurrentRound.Start();
        //        while (game.CurrentRound.IsRunning)
        //        {
        //            game.GetPlayerInput();
        //            if(game.DealerHandValue < 17 && (game.Players[0].Hand.GetValue() < 21 || game.Players[1].Hand.GetValue() < 21))
        //            {
        //                game.CurrentRound.Dealer.DrawCard();
        //            }
        //            Console.Clear();
        //            CalculateWinnings(game);
        //            game.PrintGameInformation(true);

        //            //Check if players have any money left.
        //            bool noMoney = false;
        //            foreach(Player player in game.Players)
        //            {
        //                if (player.Cash < game.MinBet) noMoney = true;
        //            }

        //            if (noMoney)
        //            {
        //                game.IsRunning = false;
        //                Console.WriteLine("\nPress any key to end game");
        //                Console.ReadKey();
        //            }
        //            else
        //            {
        //                Console.WriteLine("\nPress any key to start a new round");
        //                Console.ReadKey();
        //                Console.Clear();
        //                game.NewRound();
        //            }
        //        }

        //    }

        //    Console.Clear();

        //    Console.WriteLine();


        //}

        public static void WriteInvisibleMessage(string msg)
        {
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.WriteLine(msg);
            Console.ForegroundColor = ConsoleColor.White;
        }

        public static void CalculateWinnings(Game game)
        {
            foreach (Player player in game.Players)
            {
                if (player.Bet > 0)
                {
                    if(player.HandValue > game.DealerHandValue || game.CurrentRound.Dealer.Hand.IsBust)
                    {
                        player.Cash += player.Bet * 2;
                        
                    }
                    else if(player.HandValue == game.DealerHandValue)
                    {
                        player.Cash += player.Bet;
                    }
                    player.Bet = 0;
                }
            }
        }
    }
}
