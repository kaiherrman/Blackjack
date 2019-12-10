using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blackjack_Server.bj
{
    class Dealer
    {
        public Deck Deck { get; set; }
        public Hand Hand { get; set; }

        public Dealer(Deck deck)
        {
            Deck = deck;
        }

        public void DealToSelf()
        {
            this.Hand = new Hand(Deck);
        }

        public void DrawCard()
        {
            Hand.Cards.Add(Deck.Draw());
        }

        public void DealToPlayer(Player player)
        {
            player.Hand = new Hand(Deck);
        }
    }
}
