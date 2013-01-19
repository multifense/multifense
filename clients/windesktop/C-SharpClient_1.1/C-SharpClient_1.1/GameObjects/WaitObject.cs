using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace C_SharpClient_1._1
{
    class WaitObject : GameObject
    {
        public int waitTime;

        public WaitObject(int time)
        {
            waitTime = time;
            typeID = 99;
        }
        public override void SetGameSession(GameSession gameSession)
        {
            throw new NotImplementedException();
        }
        public override bool Update(ref List<GameObject> listToPrint)
        {
            return false;
        }
    }
}
