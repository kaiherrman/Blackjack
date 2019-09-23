using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blackjack
{
    class Player
    {
        public string Name { get; set; }
        public int Cash { get; set; }
        public Hand Hand { get; set; }

        public Player(string name, int startingCash = 10000)
        {
            this.Name = name;
            this.Cash = startingCash;
        }
    }
}
