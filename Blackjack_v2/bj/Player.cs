namespace Blackjack_Server.bj
{
    class Player
    {
        public string Name { get; }
        public int Cash { get; set; }
        public Hand Hand { get; set; }
        public int Bet { get; set; }
        public string LastMove { get; set; }
        public int HandValue => Hand.GetValue();

        public Player(string name)
        {
            Name = name;
            Cash = 10000;
        }

        public void DrawCard(Deck deck)
        {
            Hand.Cards.Add(deck.Draw());
        }

        public void PlaceBet(int amount)
        {
            if (amount < Game.MinBet) return;
            else if (amount > Game.MaxBet) return;
            else if (amount > Cash) return;
            else
            {
                Cash -= amount; //Subtract bet from cash
                Bet += amount; //Save bet to player
            }
        }
    }
}
