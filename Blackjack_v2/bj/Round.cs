using System;

namespace Blackjack_Server.bj
{
    class Round
    {
        private Game Game { get; }
        private static readonly Deck Deck = new Deck();
        public readonly Dealer Dealer = new Dealer(Deck);
        public int CurrentTurn;
        public bool IsRunning;

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
    }
}
