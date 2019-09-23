using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blackjack
{
    class Card
    {
        public Number Number { get; set; }
        public Suit Suit { get; set; }
        public int Value { get; set; }

        public Card(Number number, Suit suit)
        {
            this.Number = number;
            this.Suit = suit;
            this.Value = GetValue(number);
        }

        public static int GetValue(Number number)
        {
            switch (number)
            {
                case Number.Two:
                    return 2;
                case Number.Three:
                    return 3;
                case Number.Four:
                    return 4;
                case Number.Five:
                    return 5;
                case Number.Six:
                    return 6;
                case Number.Seven:
                    return 7;
                case Number.Eight:
                    return 8;
                case Number.Nine:
                    return 9;
                case Number.Ten:
                case Number.Jack:
                case Number.Queen:
                case Number.King:
                    return 10;
                case Number.Ace:
                    return 11;
                default:
                    throw new ArgumentOutOfRangeException(nameof(number), number, null);
            }
        }

        public string Display()
        {
            return this.Number.ToString() + " of " + this.Suit.ToString();
        }
    }

    #region enum
    public enum Number
    {
        Two,
        Three,
        Four,
        Five,
        Six,
        Seven,
        Eight,
        Nine,
        Ten,
        Jack,
        Queen,
        King,
        Ace
    }

    public enum Suit
    {
        Diamonds,
        Hearts,
        Spades,
        Clubs
    }
    #endregion
}
