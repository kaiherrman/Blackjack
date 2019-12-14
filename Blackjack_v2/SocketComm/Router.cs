using Blackjack_Server.bj;
using Newtonsoft.Json.Linq;

namespace Blackjack_Server.SocketComm
{
    class Router
    {
        private AsyncServer Server { get; }

        public Router(AsyncServer server)
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
                case "resetGame":
                    return ResetGame();
                case "newPlayer":
                    return NewPlayer((string)request.SelectToken("data").SelectToken("name"));
                case "playerAction":
                    return PlayerAction(request.SelectToken("data"));
            }

            return new JObject();
        }

        private JObject NewPlayer(string name)
        {
            Player player = new Player(name);

            if (Server.Game.Players.Count < 2)
            {
                Server.Game.AddPlayer(player);
            }
            else
            {
                return GetStatus();
            }

            if (Server.Game.Players.Count == 2)
            {
                Server.Game.NewRound();
            }

            JObject response = new JObject(
                new JProperty("clientId", Server.Game.Players.IndexOf(player))
            );

            return response;
        }

        private JObject PlaceBet(int clientId, int bet)
        {
            if(Server.Game.CurrentRound == null)
            {
                return new JObject(new JProperty("error", "not_enough_players"));
            }

            Server.Game.Players[clientId].PlaceBet(bet);

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

            if (Server.Game.CurrentRound.CurrentTurn != clientId && actionType != "bet" && actionType != "newRound")
            {
                return new JObject(new JProperty("error", "not_your_turn"));
            }
            if(!Server.Game.CurrentRound.IsRunning && actionType != "bet")
            {
                return new JObject(new JProperty("error", "round_not_started"));
            }

            Server.Game.Players[clientId].LastMove = actionType;

            switch (actionType)
            {
                case "bet":
                    if (Server.Game.HaveAllPlayersBet)
                    {
                        return new JObject(new JProperty("error", "bets_are_closed"));
                    }
                    int bet = (int)data.SelectToken("action").SelectToken("value");
                    return PlaceBet(clientId, bet);
                case "hit":
                    Server.Game.Players[clientId].DrawCard(Server.Game.CurrentRound.Dealer.Deck);
                    if (Server.Game.Players[clientId].Hand.IsBlackjack)
                    {
                        Server.Game.Players[clientId].Cash += Server.Game.Players[clientId].Bet * 3;
                        Server.Game.Players[clientId].Bet = 0;
                    }
                    else if(Server.Game.Players[clientId].Hand.IsBust)
                    {
                        Server.Game.Players[clientId].Bet = 0;
                    }
                    return GetStatus();
                case "stand":
                    if(clientId == 0)
                    {
                        Server.Game.CurrentRound.CurrentTurn = 1;
                    }
                    else
                    {
                        Server.Game.CurrentRound.CurrentTurn = -1;
                        Server.Game.CalculateWinnings();
                    }
                    return GetStatus();
                case "newRound":
                    Server.Game.Players[clientId].LastMove = "";

                    if (Server.Game.CurrentRound.CurrentTurn != -1)
                    {
                        return new JObject(new JProperty("error", "round_still_running"));
                    }

                    bool allPlayersReady = true;
                    foreach(Player player in Server.Game.Players)
                    {
                        if (player.LastMove != "")
                        {
                            allPlayersReady = false;
                        }
                    }

                    if (allPlayersReady)
                    {
                        Server.Game.NewRound();
                    }

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
                new JProperty("currentTurn", Server.Game.CurrentRound.CurrentTurn),
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

                dealer.Add("cards", cards);

                status.Add("dealer", dealer);
            }

            return status;
        }

        private JObject ResetGame()
        {
            Server.Game = new Game();
            return new JObject(new JProperty("success", "new_game_started"));
        }
    }
}
