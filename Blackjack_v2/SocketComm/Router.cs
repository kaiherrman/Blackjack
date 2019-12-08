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
                //Example Route
                case "status":
                    return GetStatus();
                case "newPlayer":
                    return NewPlayer((string)request.SelectToken("data").SelectToken("name"));
            }

            return new JObject();
        }

        private JObject NewPlayer(string name)
        {
            Server.Game.AddPlayer(new bj.Player(name));
            return GetStatus();
        }

        private JObject GetStatus()
        {
            return new JObject(JObject.Parse("{\"currentTurn\":0,\"dealer\":{\"cards\":[\"Ace of Spades\"]},\"players\":[{\"id\":0,\"name\":\"Kai\",\"bet\":500,\"cash\":10000,\"cards\":[\"2 of Hearts\",\"3 of Spades\"]},{\"id\":1,\"name\":\"David\",\"bet\":500,\"cash\":10000,\"cards\":[\"2 of Hearts\",\"3 of Spades\"]}]}"));
        }
    }
}
