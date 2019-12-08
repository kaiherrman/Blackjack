using Blackjack_v2.bj;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blackjack_v2.SocketComm
{
    class Router
    {
        Server Server { get; set; }

        public Router(Server server)
        {
            Server = server;
        }

        public JObject Route(JObject request)
        {
            string route = (string)request.SelectToken("route");
            switch (route)
            {
                case "status":
                    return GetStatus();
                case "newPlayer":
                    return NewPlayer((string)request.SelectToken("data").SelectToken("name"));
                case "playerAction":
                    return PlayerAction(request.SelectToken("data"));
            }

            return new JObject();
        }

        private JObject NewPlayer(string name)
        {
            Server.Game.AddPlayer(new Player(name));

            if(Server.Game.Players.Count == 2)
            {
                Server.Game.NewRound();
            }

            return GetStatus();
        }

        private JObject PlaceBet(int clientId, int bet)
        {
            if(Server.Game.CurrentRound == null)
            {
                return new JObject(new JProperty("error", "not_enough_players"));
            }

            Server.Game.Players[clientId].PlaceBet(bet, Server.Game);

            if (Server.Game.HaveAllPlayersBet)
            {
                Server.Game.CurrentRound.Start();
            }

            return GetStatus();
        }

        private JObject PlayerAction(JToken data)
        {
            int clientId = (int)data.SelectToken("clientId");
            string actionType = (string)data.SelectToken("action").SelectToken("type");


            Server.Game.Players[clientId].LastMove = actionType;

            switch (actionType)
            {
                case "bet":
                    int bet = (int)data.SelectToken("action").SelectToken("value");
                    return PlaceBet(clientId, bet);
                case "hit":
                    return GetStatus();
                case "stand":
                    return GetStatus();
            }

            return GetStatus();
        }

        private JObject GetStatus() 
        {
            JArray players = new JArray();

            int i = 0;
            foreach(Player player in Server.Game.Players)
            {
                JObject temp = new JObject(
                    new JProperty("id", i),
                    new JProperty("name", player.Name),
                    new JProperty("cash", player.Cash)
                );

                if(player.Bet != 0) temp.Add("bet", player.Bet);

                if (player.Hand != null)
                {
                    temp.Add("blackjack", player.Hand.IsBlackjack);
                    temp.Add("bust", player.Hand.IsBust);
                    JArray cards = new JArray();
                    foreach (Card card in player.Hand.Cards)
                    {
                        cards.Add(new JObject(
                            new JProperty("suit", card.Suit.ToString()),
                            new JProperty("number", card.Number.ToString())
                        ));
                    }
                    temp.Add("cards", cards);
                }
                i++;
                players.Add(temp);
            }


            JObject status = new JObject(
                new JProperty("currentTurn", 0),
                new JProperty("players", players)
            );

            if(Server.Game.CurrentRound != null && Server.Game.CurrentRound.IsRunning)
            {
                JObject dealer = new JObject(
                    new JProperty("blackjack", Server.Game.CurrentRound.Dealer.Hand.IsBlackjack),
                    new JProperty("bust", Server.Game.CurrentRound.Dealer.Hand.IsBust)
                );

                JArray cards = new JArray();
                foreach (Card card in Server.Game.CurrentRound.Dealer.Hand.Cards)
                {
                    cards.Add(new JObject(
                        new JProperty("suit", card.Suit.ToString()),
                        new JProperty("number", card.Number.ToString())
                    ));
                }

                status.Add("dealer", dealer);
            }

            return status;
        }
    }
}
