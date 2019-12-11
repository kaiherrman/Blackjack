using System.Collections.Generic;

namespace Blackjack_Server.bj
{
    class Game
    {
        public readonly List<Player> Players = new List<Player>();
        public const int MinBet = 5;
        public const int MaxBet = 500;
        public bool HaveAllPlayersBet => AllPlayersBet();
        public Round CurrentRound { get; private set; }
        private int DealerHandValue => CurrentRound.Dealer.Hand.GetValue();

        public void AddPlayer(Player player)
        {
            Players.Add(player);
        }

        public void NewRound()
        {
            foreach(Player player in Players)
            {
                player.Bet = 0;
                player.LastMove = "";
            }
            CurrentRound = new Round(this);
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
