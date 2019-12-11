using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blackjack_Server.bj
{
    class Game
    {
        public List<Player> Players = new List<Player>();
        public bool IsRunning = false;
        public int MinBet = 5;
        public int MaxBet = 500;
        public bool HaveAllPlayersBet => AllPlayersBet();
        public Round CurrentRound { get; set; }
        public int DealerHandValue => CurrentRound.Dealer.Hand.GetValue();

        public void AddPlayer(Player player)
        {
            Players.Add(player);
        }

        public Round NewRound()
        {
            foreach(Player player in Players)
            {
                player.Bet = 0;
                player.LastMove = "";
            }
            CurrentRound = new Round(this);
            IsRunning = true;
            return this.CurrentRound;
        }

        public void GetPlayerInput()
        {
        //    foreach (Player player in Players)
        //    {
        //        while (player.LastMove != 'S' && player.Bet > 0)
        //        {
        //            Console.Clear();
        //            PrintGameInformation();
        //            Console.Write(Environment.NewLine);
        //            Console.WriteLine("{0}, it's your turn.", player.Name);
        //            Console.Write("Controls: ");
        //            Console.Write("[H]-Hit | [S]-Stay");

        //            char key = Console.ReadKey().KeyChar;

        //            player.LastMove = Char.ToUpper(key);
        //            switch (Char.ToUpper(key))
        //            {
        //                case 'H':
        //                    Console.WriteLine("Hit");
        //                    player.DrawCard(CurrentRound.Dealer.Deck);
        //                    Console.WriteLine(player.Hand.GetValue());
        //                    if (player.Hand.IsBlackjack)
        //                    {
        //                        Console.WriteLine("BLACKJACK!");
        //                        player.Cash += player.Bet * 2;
        //                        player.Bet = 0;
        //                    }
        //                    else if (player.Hand.IsBust)
        //                    {
        //                        Console.WriteLine("You are busted");
        //                        player.Bet = 0;
        //                    }
        //                    break;
        //                case 'S':
        //                    Console.WriteLine("Stay");
        //                    break;
        //                default:
        //                    break;
        //            }
        //        }
        //    }
        }

        private bool AllPlayersBet()
        {
            bool response = true;
            foreach(Player player in Players)
            {
                if (player.Bet == 0) response = false;
            }

            return response;
        }

        public void CalculateWinnings()
        {
            if (DealerHandValue < 17 && (Players[0].Hand.GetValue() < 21 || Players[1].Hand.GetValue() < 21))
            {
                CurrentRound.Dealer.DrawCard();
            }
            foreach (Player player in Players)
            {
                if (player.Bet > 0)
                {
                    if (player.HandValue > DealerHandValue || CurrentRound.Dealer.Hand.IsBust)
                    {
                        player.Cash += player.Bet * 2;

                    }
                    else if (player.HandValue == DealerHandValue)
                    {
                        player.Cash += player.Bet;
                    }
                    player.Bet = 0;
                }
            }
        }
    }
}
