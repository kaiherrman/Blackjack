using System;
using System.Collections.Generic;

namespace Blackjack
{
    class Program
    {
        static void Main(string[] args)
        {
            Deck deck = new Deck(); //Create Deck
            Dealer dealer = new Dealer(deck); //Create Dealer

            List<Player> players = new List<Player>();

            //Create 2 players
            players.Add(new Player("Max"));
            players.Add(new Player("Dudeson"));

            Console.WriteLine("Dealer Cards:");
            Console.WriteLine("--------------------------");
            Console.WriteLine(dealer.Hand.Cards[0].Display());
            Console.WriteLine("--------------------------");
            Console.WriteLine("");


            foreach (Player player in players)
            {
                dealer.DealCards(player);
                Console.WriteLine("Player [{0}] Cards:", player.Name);
                Console.WriteLine("--------------------------");
                foreach (Card card in player.Hand.Cards)
                {
                    Console.WriteLine(card.Display());
                }
                Console.WriteLine("--------------------------");
                Console.WriteLine("");
            }
        }
    }
}
