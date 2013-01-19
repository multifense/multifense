using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace C_SharpClient_1._1
{
    class Player
    {
        public int hp = 10;
        public int gold = 90;
        public int monsterkilled = 0;
        public bool isSinglePlayer = true;
        public string playerName = "Windows";
        public int id;

        public Player(string playerName, int id)
        {
            this.playerName = playerName;
            this.id = id;
            gold = 100;
            hp = 10;
            monsterkilled = 0;
        }
    }
}
