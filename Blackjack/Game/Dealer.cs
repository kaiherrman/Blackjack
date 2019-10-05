using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blackjack
{
    class Dealer
    {
        public Deck Deck { get; set; }
        public Hand Hand { get; set; }

        public Dealer()
        {
            this.Deck = new Deck();
        }

        public Dealer(Deck deck)
        {
            this.Deck = deck;
        }

        public void DealToSelf()
        {
            this.Hand = new Hand(this.Deck);
        }

        public void DealCards(Player player)
        {
            player.Hand = new Hand(this.Deck);
        }
    }
}
