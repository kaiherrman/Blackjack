using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blackjack_v2.bj
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

        public void PrintGameInformation(bool revealDealerCards = false)
        {
            Console.WriteLine("---------- PLAYERS ----------");

            if(CurrentRound != null)
            {
                Console.WriteLine("{0} \t", "Dealer");
                if (revealDealerCards)
                {
                    Console.WriteLine("Dealer Value: {0}", CurrentRound.Dealer.Hand.GetValue());
                    foreach (Card card in CurrentRound.Dealer.Hand.Cards)
                    {
                        Console.WriteLine("   {0}", card.Display());
                    }
                }
                else
                {
                    Program.WriteInvisibleMessage("Dealer Value: " + CurrentRound.Dealer.Hand.GetValue());
                    Console.WriteLine("   " + CurrentRound.Dealer.Hand.Cards.First().Display());
                    Program.WriteInvisibleMessage("   " + CurrentRound.Dealer.Hand.Cards[1].Display());
                }
                if (CurrentRound.Dealer.Hand.IsBlackjack) Console.Write("\t BLACKJACK");
                if (CurrentRound.Dealer.Hand.IsBust) Console.Write("\t BUSTED");
                Console.Write(Environment.NewLine);
            }
            Player last = Players.Last();
            foreach (Player player in Players)
            {
                Console.Write("{0} \t", player.Name);
                Console.Write("Cash: {0}\t", player.Cash);
                if (player.Bet != 0)
                {
                    Console.Write("Bet: {0}", player.Bet);
                }
                Console.Write(Environment.NewLine);
                if (player.Hand != null)
                {
                    Console.Write("Value: {0}", player.Hand.GetValue());
                    if (player.Hand.IsBlackjack) Console.Write("\t BLACKJACK");
                    if (player.Hand.IsBust) Console.Write("\t BUSTED");
                    Console.Write(Environment.NewLine);
                    foreach (Card card in player.Hand.Cards)
                    {
                        Console.WriteLine("   {0}", card.Display());
                    }
                }
                if (player != last)
                {
                    Console.Write(Environment.NewLine);
                }
            }
            Console.WriteLine("-----------------------------");
        }

        public void GetInitialBets()
        {
            foreach (Player player in Players) //Loop for placing bets!
            {
                while (player.Bet == 0)
                {
                    Console.Write("{0}, place your bet: ", player.Name);
                    int bet = Convert.ToInt32(Console.ReadLine());
                    player.PlaceBet(bet, this);
                }
            }
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
