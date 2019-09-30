using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blackjack
{
    public class Router
    {
        public static void Route(string route, JObject data)
        {
            switch (route)
            {
                //Example Route
                case "newPlayer":
                    newPlayer(data);
                    return;
            }
        }

         
        private static void newPlayer(JObject data)
        {
            string name = (string)data.SelectToken("name");
            Console.WriteLine(name);
        }
    }
}
