using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blackjack
{
    class Round
    {
        Dealer Dealer { get; set; }
        Player[] Players { get; set; }

        public Round(Dealer dealer, Player[] players)
        {
            this.Dealer = dealer;
            this.Players = players;
        }

        public void StartRound()
        {
            Dealer.DealToSelf();
            foreach (Player player in Players)
            {
                Dealer.DealCards(player);
            }
        }

    }
}
