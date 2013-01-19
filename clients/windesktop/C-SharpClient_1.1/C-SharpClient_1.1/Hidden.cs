using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace C_SharpClient_1._1
{

    //inherits sprite
    class Hidden: GameObject
    {
        /// <summary>
        /// A class used for you to create rectagnles at areas you will not want to build
        /// The type is the cool and secret Ermac(hidden)
        /// </summary>
        /// <param name="rec">The area you want to be unable to build on!</param>
        public Hidden(Rectangle rec)
        {
            //It receives a rectangle, with x,y coordinates and how big it is. for example(0,0,100,100) a rectangle at (0,0) with 100x100 dimensions
            //base = super (in java)
            base.Rec = rec;
            base.type = "Ermac";
        }
        public Hidden(Rectangle rec, SpriteFont sp)
        {
            base.font1text = sp;
            base.strouput = " hej";
        }

        public override bool Update(ref List<GameObject> listToPrint)
        {
            return false;
        }
        public override void SetGameSession(GameSession gameSession)
        {
            base.gameSession = gameSession;
        }
    }
}
