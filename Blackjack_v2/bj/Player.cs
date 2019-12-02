using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blackjack_v2.bj
{
    class Player
    {
        public string Name { get; set; }
        public int Cash { get; set; }
        public Hand Hand { get; set; }
        public int Bet { get; set; }
        public char LastMove { get; set; }
        public int HandValue => Hand.GetValue();

        public Player(string name)
        {
            Name = name;
            Cash = 10000;
        }

        public Player(string name, int cash)
        {
            Name = name;
            Cash = cash;
        }

        public void DrawCard(Deck deck)
        {
            Hand.Cards.Add(deck.Draw());
        }

        public void PlaceBet(int amount, Game game)
        {
            if (amount < game.MinBet) Console.WriteLine("Bet is too low");
            else if (amount > game.MaxBet) Console.WriteLine("Bet is too high");
            else if (amount > Cash) Console.WriteLine("Not enough cash");
            else
            {
                Cash -= amount; //Subtract bet from cash
                Bet += amount; //Save bet to player
            }
        }
    }
}
