using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blackjack
{
    class Router
    {
        public Game.Game Game = new Game.Game();

        public JObject Route(string route, JObject data)
        {
            switch (route)
            {
                //Example Route
                case "newPlayer":
                    return NewPlayer(data);
                case "gameStatus":
                    return GameStatus();
            }

            return new JObject();
        }
         
        private JObject NewPlayer(JObject data)
        {
            string name = (string)data.SelectToken("name");
            Console.WriteLine("[NEW PLAYER] {0}", name);

            Game.AddPlayer(new Player(name));

            return createResponse(true, "Successfully added Player.");
        }

        private JObject GameStatus()
        {
            return Game.Status;
        }

        private JObject createResponse(bool success, string message)
        {
            return new JObject(
                new JProperty("success", success),
                new JProperty("message", message)
            );
        }
    }
}
