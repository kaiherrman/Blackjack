using System;
using System.Collections.Generic;

namespace Blackjack_Server.bj
{
    class Deck
    {
        private readonly List<Card> _cards = new List<Card>();
        private readonly Random _random = new Random();

        public Deck()
        {
            foreach (Suit suit in Enum.GetValues(typeof(Suit)))
            {
                foreach (Number number in Enum.GetValues(typeof(Number))){
                    _cards.Add(new Card(number, suit));
                }
            }
        }

        public Card Draw()
        {
            if(_cards.Count == 0)
            {
                throw new Exception("There are only 52 cards my dude.");
            }
            int randomNumber = _random.Next(0, _cards.Count);
            Card card = _cards[randomNumber];
            _cards.RemoveAt(randomNumber);
            return card;
        }
    }
}
