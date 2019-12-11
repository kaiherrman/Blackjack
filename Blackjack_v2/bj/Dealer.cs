namespace Blackjack_Server.bj
{
    class Dealer
    {
        public Deck Deck { get; }
        public Hand Hand { get; private set; }

        public Dealer(Deck deck)
        {
            Deck = deck;
        }

        public void DealToSelf()
        {
            this.Hand = new Hand(Deck);
        }

        public void DrawCard()
        {
            Hand.Cards.Add(Deck.Draw());
        }

        public void DealToPlayer(Player player)
        {
            player.Hand = new Hand(Deck);
        }
    }
}
