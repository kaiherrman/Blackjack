using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blackjack_v2.bj
{
    class Round
    {
        private Game Game { get; set; }
        public static Deck Deck = new Deck();
        public Dealer Dealer = new Dealer(Deck);
        public bool IsRunning = false;
        public Player Winner { get; set; }

        public Round(Game game)
        {
            Game = game;
            this.Game.PrintGameInformation();
            Game.GetInitialBets();
        }

        public void Start()
        {
            IsRunning = true;
            Dealer.DealToSelf();
            Program.WriteInvisibleMessage("Dealer Value: " + Dealer.Hand.GetValue());

            Console.WriteLine("{0}", Dealer.Hand.Cards.First().Display());

            if (Dealer.Hand.IsBlackjack())
            {
                Console.WriteLine("Dealer has BlackJack");
                foreach(Player player in Game.Players)
                {
                    player.Bet = 0;
                }
                Game.NewRound();
            }

            foreach(Player player in Game.Players)
            {
                Dealer.DealToPlayer(player);
            }
        }

        public void Stop()
        {
            IsRunning = false;
        }
    }
}
