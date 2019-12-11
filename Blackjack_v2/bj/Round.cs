using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blackjack_Server.bj
{
    class Round
    {
        private Game Game { get; set; }
        public static Deck Deck = new Deck();
        public Dealer Dealer = new Dealer(Deck);
        public int CurrentTurn = 0;
        public bool IsRunning = false;

        public Round(Game game)
        {
            Game = game;
        }

        public void Start()
        {
            IsRunning = true;
            Dealer.DealToSelf();
            if (Dealer.Hand.IsBlackjack)
            {
                Console.WriteLine("Dealer has BlackJack");
                foreach(Player player in Game.Players)
                {
                    player.Bet = 0;
                }
            }
            foreach(Player player in Game.Players)
            {
                Dealer.DealToPlayer(player);
                if (player.Hand.IsBlackjack)
                {
                    player.Cash += player.Bet * 3;
                    player.Bet = 0;
                    if(CurrentTurn == 0 && Game.Players.IndexOf(player) == 0)
                    {
                        CurrentTurn = 1;
                    }
                }
            }

        }

        public void Stop()
        {
            IsRunning = false;
        }
    }
}
