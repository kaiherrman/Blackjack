using Newtonsoft.Json.Linq;
using System.Linq;

namespace Blackjack.Game
{
    class Game
    {
        private Player[] Players = new Player[2];
        public Dealer Dealer { get; set; }
        public Round CurrentRound { get; set; }
        public JObject Status { get; set; }

        public Game()
        {
        }

        public void StartGame()
        {
            this.Dealer = new Dealer();
            this.CurrentRound = new Round(this.Dealer, this.Players, this);
        }

        public void AddPlayer(Player player)
        {
            this.Players.Append(player);
            if(Players.Length == 2)
            {
                StartGame();
            }
        }
    }
}
