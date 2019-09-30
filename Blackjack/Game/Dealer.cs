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
            this.Hand = new Hand(this.Deck);
        }

        public Dealer(Deck deck)
        {
            this.Deck = deck;
            this.Hand = new Hand(this.Deck);
        }

        public void DealCards(Player player)
        {
            player.Hand = new Hand(this.Deck);
        }
    }
}
