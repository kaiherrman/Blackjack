using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace Blackjack
{
    class Round
    {
        Dealer Dealer { get; set; }
        Player[] Players { get; set; }
        Game.Game Game { get; set; }

        public Round(Dealer dealer, Player[] players, Game.Game game)
        {
            this.Dealer = dealer;
            this.Players = players;
            this.Game = game;
        }

        public void StartRound()
        {
            Dealer.DealToSelf();
            foreach (Player player in Players)
            {
                Dealer.DealCards(player);
            }

            this.Game.Status = new JObject(
                new JProperty("Cards", new JObject())
            );
        }
    }
}
