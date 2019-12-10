﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blackjack_Server.bj
{
    class Hand
    {
        public List<Card> Cards = new List<Card>();
        public bool IsBlackjack => GetValue() == 21;
        public bool IsBust => GetValue() > 21;

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
    }
}
