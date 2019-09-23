using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blackjack
{
    class Hand
    {
        public List<Card> Cards = new List<Card>();

        public Hand(Deck deck)
        {
            for(int i = 0; i < 2; i++)
            {
                Cards.Add(deck.Draw());
            }
        }

    }
}
