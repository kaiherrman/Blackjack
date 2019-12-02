using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blackjack_v2.bj
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
        
        public int GetValue()
        {
            int value = 0;
            foreach(Card card in this.Cards)
            {
                value += card.Value;
            }

            return value;
        }

        public bool IsBlackjack()
        {
            return GetValue() == 21;
        }
    }
}
