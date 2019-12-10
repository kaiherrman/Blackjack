using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blackjack_Server.bj
{
    class Deck
    {
        public List<Card> Cards = new List<Card>();
        public Random random = new Random();

        public Deck()
        {
            foreach (Suit suit in Enum.GetValues(typeof(Suit)))
            {
                foreach (Number number in Enum.GetValues(typeof(Number))){
                    Cards.Add(new Card(number, suit));
                }
            }
        }

        public Card Draw()
        {
            if(Cards.Count == 0)
            {
                throw new Exception("There are only 52 cards my dude.");
            }
            int randomNumber = random.Next(0, Cards.Count);
            Card card = Cards[randomNumber];
            Cards.RemoveAt(randomNumber);
            return card;
        }
    }
}
