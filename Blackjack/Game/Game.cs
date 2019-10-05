using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            this.Dealer = new Dealer();
        }

        public void StartGame()
        {
            this.CurrentRound = new Round(this.Dealer, this.Players);

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
